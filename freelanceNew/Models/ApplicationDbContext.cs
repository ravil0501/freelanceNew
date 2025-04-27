using Microsoft.EntityFrameworkCore;

namespace freelanceNew.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet Properties
        public DbSet<User> Users { get; set; }
        public DbSet<FreelancerProfile> FreelancerProfiles { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<FreelancerSkill> FreelancerSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for FreelancerSkill
            modelBuilder.Entity<FreelancerSkill>()
                .HasKey(fs => new { fs.FreelancerId, fs.SkillId });

            modelBuilder.Entity<ClientProfile>()
                .HasKey(cp => cp.ClientId);

            // Configure one-to-one relationship between User and FreelancerProfile
            modelBuilder.Entity<User>()
                .HasOne(u => u.FreelancerProfile)
                .WithOne(fp => fp.User)
                .HasForeignKey<FreelancerProfile>(fp => fp.FreelancerId);

            // Configure one-to-one relationship between User and ClientProfile
            modelBuilder.Entity<User>()
                .HasOne(u => u.ClientProfile)
                .WithOne(cp => cp.User)
                .HasForeignKey<ClientProfile>(cp => cp.ClientId);

            // Configure relationships for Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships for Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsGiven)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewee)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(r => r.RevieweeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
