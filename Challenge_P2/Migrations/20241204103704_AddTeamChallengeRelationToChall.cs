﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Challenge_P2.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamChallengeRelationToChall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageB64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuideUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_IdType",
                        column: x => x.IdType,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamChallenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamChallenges_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamChallenges_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeQuestions",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeQuestions", x => new { x.ChallengeId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_ChallengeQuestions_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => new { x.UserId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeams_Utilisateurs_UserId",
                        column: x => x.UserId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Challenges",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "DifficultyLevel", "GuideUrl", "ImageB64", "Points", "Title" },
                values: new object[] { 1, "Phishing", new DateTime(2024, 12, 4, 11, 37, 4, 235, DateTimeKind.Local).AddTicks(5206), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque pulvinar massa faucibus malesuada pharetra. Praesent malesuada magna at mi rutrum fringilla. Nam ut risus et erat consectetur convallis. Nunc consectetur nisi turpis, ac molestie urna cursus ut. Pellentesque efficitur id odio eget tristique. Vestibulum mi diam, maximus sed libero vel, varius sagittis eros. Mauris vestibulum lorem nec libero faucibus laoreet. Nulla magna odio, malesuada vel volutpat sollicitudin, rhoncus sit amet neque. Suspendisse convallis tellus porttitor congue luctus. Proin tempus dui facilisis est auctor, vel viverra urna eleifend. Vivamus ac dui a nisl porta vestibulum. Pellentesque justo ante, pellentesque vel sapien ac, mollis rhoncus leo.", "1", "https://google.com", "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", 15, "Mail me" });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Multiple Choice" },
                    { 2, "True/False" },
                    { 3, "Single Choice" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("cfda1219-7276-455d-8aaa-42b57945e92c"), "Default Team" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "IdType", "Text", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Phishing involves attempting to acquire sensitive information by pretending to be a trustworthy entity.", "What is Phishing?" },
                    { 2, 1, "A brute force attack attempts to gain access to accounts by trying all possible combinations of passwords.", "What is a Brute Force Attack?" }
                });

            migrationBuilder.InsertData(
                table: "Utilisateurs",
                columns: new[] { "Id", "CreatedAt", "Email", "Password", "ProfilePicture", "RoleId", "Username" },
                values: new object[,]
                {
                    { new Guid("43634094-a6e8-4c6e-8bef-5e2c44c14013"), new DateTime(2024, 12, 4, 11, 37, 4, 231, DateTimeKind.Local).AddTicks(2994), "Pierre@gmail.com", "$2a$12$ZzctyUVnJ/i32vZJIwBM0eRMOxV6U60nyzO2jO7kV426p3xjnWqpq", "https://pixabay.com/vectors/blank-profile-picture", 1, "Pierre" },
                    { new Guid("d9e14cd0-8be1-43ce-ac0c-0233691d5006"), new DateTime(2024, 12, 4, 11, 37, 3, 663, DateTimeKind.Local).AddTicks(6283), "Jean@gmail.com", "$2a$12$8p.dPkv8LMtEYdTZ4f7XHOI/c3iQ/tdlYowPc9JDVWfbFZyvXzHRe", "https://pixabay.com/vectors/blank-profile-picture-mystery-man-973460/", 1, "Jean" }
                });

            migrationBuilder.InsertData(
                table: "ChallengeQuestions",
                columns: new[] { "ChallengeId", "QuestionId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "QuestionOptions",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 1, true, 1, "A type of cybersecurity attack" },
                    { 2, false, 1, "A method to encrypt files" },
                    { 3, true, 2, "An attack involving guessing passwords" },
                    { 4, false, 2, "A method to increase system performance" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestions_QuestionId",
                table: "ChallengeQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_IdType",
                table: "Questions",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_TeamChallenges_ChallengeId",
                table: "TeamChallenges",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamChallenges_TeamId",
                table: "TeamChallenges",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamId",
                table: "UserTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_RoleId",
                table: "Utilisateurs",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeQuestions");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "TeamChallenges");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Challenges");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
