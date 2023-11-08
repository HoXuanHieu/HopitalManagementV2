using AutoMapper;
using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.AppointmentDTOs;
using HospitalManagement.API.Repositories.Appointment;
using HospitalManagement.API.Repositories.Doctor;
using HospitalManagement.API.Repositories.User;
using HospitalManagement.API.Services.Email;

namespace HospitalManagement.API.Services.Appointment
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IEmailService emailService,
            IUserRepository userRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _emailService = emailService;
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<APIResponse> CreateAppointment(AppointmentCreateDTO appointmentCreate)
        {
            try
            {
                var patient = await _userRepository.GetUser(appointmentCreate.UserId.Value);
                var doctor = await _userRepository.GetUser(appointmentCreate.DoctorId.Value);
                if (patient == null || patient.Role.Name != "User")
                {
                    return new APIResponse
                    {
                        StatusCode = 404,
                        Message = "Can not find Patient by this patient id"
                    };
                }
                if(doctor == null || doctor.Role.Name != "Doctor")
                {
                    return new APIResponse
                    {
                        StatusCode = 404,
                        Message = "Can not find Patient by this doctor id"
                    };
                }
                // Tạo appointment
                var appointment = _mapper.Map<AppointmentCreateDTO, Models.Appointment>(appointmentCreate);
                appointment.Date = DateTime.Now;
                await _appointmentRepository.CreateAppointmentAsync(appointment);
                await _appointmentRepository.IsSaveChange();
                return new APIResponse
                {
                    StatusCode = 200,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }

        }

        public async Task<APIResponse> DeleteAppointment(int id, string role, int userId)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentById(id);
                if (appointment == null)
                {
                    return new APIResponse
                    {
                        StatusCode = 404,
                        Message = "Không tồn tại cuộc hẹn này"
                    };
                }
                if (role == "User" && appointment.UserId != userId) return new APIResponse
                {
                    StatusCode = 400,
                    Message = "Bạn không thể xóa lịch khám của bệnh nhân khác"
                };
                await _appointmentRepository.DeleteAppointment(appointment);
                await _appointmentRepository.IsSaveChange();
                return new APIResponse
                {
                    StatusCode = 200,
                    Message = "Thành công"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse> GetAppointmentById(int id)
        {

            try
            {
                var res = await _appointmentRepository.GetAppointmentById(id);
                var resDto = _mapper.Map<Models.Appointment, AppointmentView>(res);
                if (res == null) return new APIResponse
                {
                    StatusCode = 404,
                    Message = $"Không tồn tại cuộc hẹn có id = {id}"
                };
                return new APIResponse
                {
                    StatusCode = 200,
                    Message = "Thành công",
                    Data = resDto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<APIResponse> GetAppointments(int? page = null, int? pageSize = null, DateTime? date = null, int? patientId = null, int? doctorId = null, string? sortBy = "Date")
        {
            try
            {
                var result = await _appointmentRepository.GetAppointments(page, pageSize, date, patientId, doctorId, sortBy);
                var resultView = _mapper.Map<PaginationDTO<Models.Appointment>, PaginationDTO<AppointmentView>>(result);
                return new APIResponse
                {
                    StatusCode = 200,
                    Message = "Thành công",
                    Data = resultView
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }

        }
    }
}
