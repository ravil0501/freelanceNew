using System.ComponentModel.DataAnnotations;

namespace freelanceNew.Models
{
    public class Skill
    {
        [Key]
        public Guid SkillId { get; set; } // Primary Key
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<FreelancerSkill> FreelancerSkills { get; set; }
    }


}
