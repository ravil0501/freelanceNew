using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.FreelancersDto
{
    public class UpdateSkillsDto
    {
        [Required]
        public List<Guid> SkillIds { get; set; } = new();
    }
}
