﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using freelanceNew.Models;

#nullable disable

namespace freelanceNew.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250428050436_FreelancerKeyAdded")]
    partial class FreelancerKeyAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("freelanceNew.Models.ClientProfile", b =>
                {
                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClientId");

                    b.ToTable("ClientProfiles");
                });

            modelBuilder.Entity("freelanceNew.Models.Contract", b =>
                {
                    b.Property<Guid>("ContractId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("AgreedRate")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientProfileClientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FreelancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FreelancerProfileFreelancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("JobId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("ContractId");

                    b.HasIndex("ClientProfileClientId");

                    b.HasIndex("FreelancerProfileFreelancerId");

                    b.HasIndex("JobId")
                        .IsUnique();

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("freelanceNew.Models.FreelancerProfile", b =>
                {
                    b.Property<Guid>("FreelancerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("numeric");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PortfolioUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FreelancerId");

                    b.ToTable("FreelancerProfiles");
                });

            modelBuilder.Entity("freelanceNew.Models.FreelancerSkill", b =>
                {
                    b.Property<Guid>("FreelancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SkillId")
                        .HasColumnType("uuid");

                    b.HasKey("FreelancerId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("FreelancerSkills");
                });

            modelBuilder.Entity("freelanceNew.Models.Job", b =>
                {
                    b.Property<Guid>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Budget")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientProfileClientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("PostedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("JobId");

                    b.HasIndex("ClientProfileClientId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("freelanceNew.Models.Message", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("MessageId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("freelanceNew.Models.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PaidAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PaymentId");

                    b.HasIndex("ContractId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("freelanceNew.Models.Proposal", b =>
                {
                    b.Property<Guid>("ProposalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CoverLetter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FreelancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FreelancerProfileFreelancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("JobId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("ProposedRate")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ProposalId");

                    b.HasIndex("FreelancerProfileFreelancerId");

                    b.HasIndex("JobId");

                    b.ToTable("Proposals");
                });

            modelBuilder.Entity("freelanceNew.Models.Review", b =>
                {
                    b.Property<Guid>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ReviewedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RevieweeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ReviewerId")
                        .HasColumnType("uuid");

                    b.HasKey("ReviewId");

                    b.HasIndex("ContractId");

                    b.HasIndex("RevieweeId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("freelanceNew.Models.Skill", b =>
                {
                    b.Property<Guid>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SkillId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("freelanceNew.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("freelanceNew.Models.ClientProfile", b =>
                {
                    b.HasOne("freelanceNew.Models.User", "User")
                        .WithOne("ClientProfile")
                        .HasForeignKey("freelanceNew.Models.ClientProfile", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("freelanceNew.Models.Contract", b =>
                {
                    b.HasOne("freelanceNew.Models.ClientProfile", "ClientProfile")
                        .WithMany("Contracts")
                        .HasForeignKey("ClientProfileClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("freelanceNew.Models.FreelancerProfile", "FreelancerProfile")
                        .WithMany("Contracts")
                        .HasForeignKey("FreelancerProfileFreelancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("freelanceNew.Models.Job", "Job")
                        .WithOne("Contract")
                        .HasForeignKey("freelanceNew.Models.Contract", "JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientProfile");

                    b.Navigation("FreelancerProfile");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("freelanceNew.Models.FreelancerProfile", b =>
                {
                    b.HasOne("freelanceNew.Models.User", "User")
                        .WithOne("FreelancerProfile")
                        .HasForeignKey("freelanceNew.Models.FreelancerProfile", "FreelancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("freelanceNew.Models.FreelancerSkill", b =>
                {
                    b.HasOne("freelanceNew.Models.FreelancerProfile", "FreelancerProfile")
                        .WithMany("FreelancerSkills")
                        .HasForeignKey("FreelancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("freelanceNew.Models.Skill", "Skill")
                        .WithMany("FreelancerSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FreelancerProfile");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("freelanceNew.Models.Job", b =>
                {
                    b.HasOne("freelanceNew.Models.ClientProfile", "ClientProfile")
                        .WithMany("Jobs")
                        .HasForeignKey("ClientProfileClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientProfile");
                });

            modelBuilder.Entity("freelanceNew.Models.Message", b =>
                {
                    b.HasOne("freelanceNew.Models.User", "Receiver")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("freelanceNew.Models.User", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("freelanceNew.Models.Payment", b =>
                {
                    b.HasOne("freelanceNew.Models.Contract", "Contract")
                        .WithMany("Payments")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("freelanceNew.Models.Proposal", b =>
                {
                    b.HasOne("freelanceNew.Models.FreelancerProfile", "FreelancerProfile")
                        .WithMany("Proposals")
                        .HasForeignKey("FreelancerProfileFreelancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("freelanceNew.Models.Job", "Job")
                        .WithMany("Proposals")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FreelancerProfile");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("freelanceNew.Models.Review", b =>
                {
                    b.HasOne("freelanceNew.Models.Contract", "Contract")
                        .WithMany("Reviews")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("freelanceNew.Models.User", "Reviewee")
                        .WithMany("ReviewsReceived")
                        .HasForeignKey("RevieweeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("freelanceNew.Models.User", "Reviewer")
                        .WithMany("ReviewsGiven")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Reviewee");

                    b.Navigation("Reviewer");
                });

            modelBuilder.Entity("freelanceNew.Models.ClientProfile", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("freelanceNew.Models.Contract", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("freelanceNew.Models.FreelancerProfile", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("FreelancerSkills");

                    b.Navigation("Proposals");
                });

            modelBuilder.Entity("freelanceNew.Models.Job", b =>
                {
                    b.Navigation("Contract")
                        .IsRequired();

                    b.Navigation("Proposals");
                });

            modelBuilder.Entity("freelanceNew.Models.Skill", b =>
                {
                    b.Navigation("FreelancerSkills");
                });

            modelBuilder.Entity("freelanceNew.Models.User", b =>
                {
                    b.Navigation("ClientProfile")
                        .IsRequired();

                    b.Navigation("FreelancerProfile")
                        .IsRequired();

                    b.Navigation("ReceivedMessages");

                    b.Navigation("ReviewsGiven");

                    b.Navigation("ReviewsReceived");

                    b.Navigation("SentMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
