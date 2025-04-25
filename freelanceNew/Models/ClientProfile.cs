using System.Diagnostics.Contracts;

namespace freelanceNew.Models
{
    public class ClientProfile
    {
        public Guid ClientId { get; set; } // Primary Key and Foreign Key to User
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }
}
