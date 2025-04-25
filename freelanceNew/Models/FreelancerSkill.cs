namespace freelanceNew.Models
{
    public class FreelancerSkill
    {
        public Guid FreelancerId { get; set; } // Foreign Key to FreelancerProfile
        public Guid SkillId { get; set; } // Foreign Key to Skill

        // Navigation Properties
        public FreelancerProfile FreelancerProfile { get; set; }
        public Skill Skill { get; set; }
    }


}
