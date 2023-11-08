using AutoMapper;
using HospitalManagement.API.DTOs;
using HospitalManagement.API.DTOs.AppointmentDTOs;
using HospitalManagement.API.DTOs.DoctorDTOs;
using HospitalManagement.API.DTOs.HospitalDTOs;
using HospitalManagement.API.DTOs.UserDTOs;
using HospitalManagement.API.Models;

namespace HospitalManagement.API
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRegisterDTO, User>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserDTO>().ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name)); ;
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserUpdateDTO>();
            CreateMap<Hospital, HospitalDTO>();
            CreateMap<HospitalDTO, Hospital>();
            CreateMap<PaginationDTO<Hospital>, PaginationDTO<HospitalDTO>>();
            CreateMap<DoctorCreateDTO, DoctorDTO>();
            CreateMap<Doctor, DoctorDTO>();
            CreateMap<PaginationDTO<Doctor>, PaginationDTO<DoctorDTO>>();
            CreateMap<Appointment, AppointmentView>();
            CreateMap<PaginationDTO<Appointment>, PaginationDTO<AppointmentView>>();
            CreateMap<AppointmentCreateDTO, Appointment>();
            CreateMap<List<Doctor>, DoctorDTO>();
            CreateMap<Hospital, HospitalDetails>();
        }

    }
}
