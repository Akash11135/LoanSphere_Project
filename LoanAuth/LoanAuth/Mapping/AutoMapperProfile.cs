using AutoMapper;
using LoanAuth.DTOs;
using LoanAuth.Models;

namespace LoanAuth.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterCustomerDto, ApplicationUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

            CreateMap<RegisterAdminDto, ApplicationUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

            CreateMap<RegisterManagerDto, ApplicationUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));
        }
    }
}