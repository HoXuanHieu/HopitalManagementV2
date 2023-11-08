namespace HospitalManagement.API.Services.Token
{
    public interface ITokenService
    {
        Task<string> CreateToken(string email);
    }
}
