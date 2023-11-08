using HospitalManagement.API.Repositories.Doctor;
using HospitalManagement.API.Repositories.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagement.API.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;

        public TokenService(IConfiguration configuration, IUserRepository userRepository, IDoctorRepository doctorRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<string> CreateToken(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Role", user.Role.Name),
                new Claim("Email", email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Name", user.FullName)
            };

            if (user.Role.Name == "Doctor")
            {
                var doctor = await _doctorRepository.GetDoctorByUserId(user.Id);
                claims.Add(new Claim("DoctorId", doctor.Id.ToString()));
            }

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
