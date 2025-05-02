using AutoMapper;
using freelanceNew.DTOModels.ClientsDto;
using freelanceNew.DTOModels.ContractsDto;
using freelanceNew.DTOModels.FreelancersDto;
using freelanceNew.DTOModels.JobsDto;
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

            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateJobDto, Job>();
            CreateMap<UpdateJobDto, Job>();

            CreateMap<ClientProfile, ClientProfileDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<CreateClientProfileDto, ClientProfile>();
            CreateMap<UpdateClientProfileDto, ClientProfile>();

            CreateMap<FreelancerProfile, FreelancerProfileDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
            src.FreelancerSkills.Select(fs => new FreelancerSkillDto
            {
            SkillId = fs.SkillId,
            SkillName = fs.Skill.Name
            })));

            CreateMap<FreelancerSkill, FreelancerSkillDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));

            // Добавь остальные маппинги по мере необходимости
        }
    }
}
