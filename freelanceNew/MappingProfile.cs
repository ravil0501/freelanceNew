using AutoMapper;
using freelanceNew.DTOModels.ContractsDto;
using freelanceNew.DTOModels.UsersDTO;
using freelanceNew.Models;

namespace freelanceNew
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Contract
            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateContractDto, Contract>();
            CreateMap<UpdateContractDto, Contract>();

            // Добавь остальные маппинги по мере необходимости
        }
    }
}
