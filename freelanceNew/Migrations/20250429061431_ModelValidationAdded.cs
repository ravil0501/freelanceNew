using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace freelanceNew.Migrations
{
    /// <inheritdoc />
    public partial class ModelValidationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerSkills_FreelancerProfiles_FreelancerId",
                table: "FreelancerSkills");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "FreelancerProfileFreelancerId",
                table: "FreelancerSkills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "FreelancerProfiles",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "FreelancerProfiles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerSkills_FreelancerProfileFreelancerId",
                table: "FreelancerSkills",
                column: "FreelancerProfileFreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerSkills_FreelancerProfiles_FreelancerProfileFreela~",
                table: "FreelancerSkills",
                column: "FreelancerProfileFreelancerId",
                principalTable: "FreelancerProfiles",
                principalColumn: "FreelancerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerSkills_FreelancerProfiles_FreelancerProfileFreela~",
                table: "FreelancerSkills");

            migrationBuilder.DropIndex(
                name: "IX_FreelancerSkills_FreelancerProfileFreelancerId",
                table: "FreelancerSkills");

            migrationBuilder.DropColumn(
                name: "FreelancerProfileFreelancerId",
                table: "FreelancerSkills");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "FreelancerProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "FreelancerProfiles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerSkills_FreelancerProfiles_FreelancerId",
                table: "FreelancerSkills",
                column: "FreelancerId",
                principalTable: "FreelancerProfiles",
                principalColumn: "FreelancerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
