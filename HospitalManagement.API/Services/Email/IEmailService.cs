using HospitalManagement.API.DTOs;

namespace HospitalManagement.API.Services.Email
{
    public interface IEmailService
    {
        APIResponse SendEmail(string to, string subject, string body);
    }
}