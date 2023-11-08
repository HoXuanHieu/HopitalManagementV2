using AutoMapper;
using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.UserDTOs;
using HospitalManagement.API.Repositories.Role;
using HospitalManagement.API.Repositories.User;
using HospitalManagement.API.Services.Email;
using HospitalManagement.API.Services.Token;

namespace HospitalManagement.API.Services.Authenticate
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailService _emailService;

        public AuthenticateService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IMapper mapper,
            IRoleRepository roleRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _emailService = emailService;
        }

        public async Task<string> Register(UserRegisterDTO userRegisterDTO)
        {
            bool check = await _userRepository.IsEmailAlreadyExists(userRegisterDTO.Email);
            if (check) throw new BadHttpRequestException("Email existed!");
            var newUser = _mapper.Map<UserRegisterDTO, Models.User>(userRegisterDTO);
            var role = await _roleRepository.GetRoleByName("User");
            newUser.RoleId = role.Id;
            newUser.Gender = true;
            newUser.IsEmailVerified = false;
            await _userRepository.CreateUser(newUser);
            var token = await _tokenService.CreateToken(newUser.Email);
            newUser.Token = token;
            string body = $"<a href='http://localhost:3000/auth/verify-account?token={token}'>Click here<a/> to authenticate your account";
            _emailService.SendEmail(userRegisterDTO.Email, "Authenticate your account", body);
            await _userRepository.UpdateUser(newUser);
            return token;
        }

        public async Task<APIResponse> Login(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.GetUserByEmail(userLoginDTO.Email);
            if (user == null)
            {
                return new APIResponse { StatusCode = 400, Message = "Email doesn't exist!" };
            }
            if (userLoginDTO.Password != user.Password)
            {
                return new APIResponse { StatusCode = 402, Message = "Password is incorrect!" };
            }
            var token = await _tokenService.CreateToken(user.Email);
            return new APIResponse { StatusCode = 200, Message = "Success", Data = token };
        }
    }
}
