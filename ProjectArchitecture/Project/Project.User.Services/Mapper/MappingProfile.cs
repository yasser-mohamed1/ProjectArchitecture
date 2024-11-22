using AutoMapper;
using Project.Comman.Idenitity;
using Project.Data.Entities;
using Project.Services.DataTransferObject.AuthenticationDto;
using Project.Services.DataTransferObject.Customer;
namespace Project.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
         
            CreateMap<RegisterUserDto, ApplicationUser>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, GetUserDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName)).ReverseMap();

            CreateMap<Customer, CustomerGetAllModel>().ReverseMap();
            CreateMap<Customer, CustomerSearchModel>().ReverseMap();
            CreateMap<CustomerCreateModel, Customer>().ReverseMap();

        }
    }
}
