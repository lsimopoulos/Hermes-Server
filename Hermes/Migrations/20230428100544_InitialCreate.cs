using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hermes.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HermesMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Delivered = table.Column<bool>(type: "bit", nullable: false),
                    Time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HermesMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HermesUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsGroup = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HermesUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_HermesUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_HermesUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_HermesUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_HermesUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HermesUserContacts",
                columns: table => new
                {
                    HermesUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HermesUserContacts", x => new { x.HermesUserId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_HermesUserContacts_HermesUsers_ContactId",
                        column: x => x.ContactId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HermesUserContacts_HermesUsers_HermesUserId",
                        column: x => x.HermesUserId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HermesUserGroups",
                columns: table => new
                {
                    HermesUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HermesUserGroups", x => new { x.HermesUserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_HermesUserGroups_HermesUsers_GroupId",
                        column: x => x.GroupId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HermesUserGroups_HermesUsers_HermesUserId",
                        column: x => x.HermesUserId,
                        principalTable: "HermesUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HermesUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsGroup", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("038da423-3391-4217-a21c-5b8ae593d52e"), 0, "a522df9b-ef72-4868-bc25-f8681f43d4ed", null, false, false, false, null, "haha3", null, "haha3@haha.com", "AQAAAAIAAYagAAAAENMCwtW/5UwYc6y/BAIFSP4LEI5jll7BaeIeLZeNtXFRNt3iT4mfy1J7MI0UlA9djg==", null, false, "7661f57b-c9c0-41d4-b318-f351024e963e", false, "haha3@haha.com" },
                    { new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07"), 0, "9a4c8fcb-6d1f-4e2a-bcd0-53a7414d929e", null, false, false, false, null, "haha0", null, "haha0@haha.com", "AQAAAAIAAYagAAAAELkUohcH2NYpJCrDnTp2IhAnmUZgCWDDNlF8dqaOmoRcKxKLJSpK78Vne/Rn9XRlsA==", null, false, "15d196ae-055c-449e-974e-f8aef922eea2", false, "haha0@haha.com" },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), 0, "0748a88b-10a2-4def-b249-0861550aee57", "testGroup@haha.com", false, true, false, null, "testGroup", null, null, null, null, false, "6f407d11-0e38-4585-bc69-11f2c8ef2792", false, "testGroup@haha.com" },
                    { new Guid("d8793545-96c6-4901-8114-78d82c01504f"), 0, "b9374037-de98-4997-be18-3a4368dbb31f", null, false, false, false, null, "haha1", null, "haha1@haha.com", "AQAAAAIAAYagAAAAEI97Km/uRYo81wmG9qJsLOFO36KrLEbsgBONSEpkC//l3nOh9FRBv9zHY4Bxg7qfPg==", null, false, "9dcad3f4-3720-49bf-8d95-0beb23e05ecf", false, "haha1@haha.com" },
                    { new Guid("e48d16be-738d-473f-8505-fcd121d8ef35"), 0, "71b1cab5-890e-4c76-8868-2d8cdc001dc7", null, false, false, false, null, "haha4", null, "haha4@haha.com", "AQAAAAIAAYagAAAAEDlb5Kvq/Bpzjr3LDep7P69KOZqFj0zIRK6y4Gk/oviSZRNEeN1eFc2hQVlilr2tNw==", null, false, "d40ac185-b6a6-4dad-a6d5-a3d08c126276", false, "haha4@haha.com" },
                    { new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df"), 0, "ed5adcf4-0b96-4587-b9b9-2ffa70e6a259", null, false, false, false, null, "haha2", null, "haha2@haha.com", "AQAAAAIAAYagAAAAEDU8UT7Oynzi2GIWXZqTQIzGqlfBdMx0Y2AdpKcATeEPfvyHJ+voWqvlUp9qRmVxcg==", null, false, "4ca81c1b-c8c9-45c7-b743-8d238dc35f77", false, "haha2@haha.com" }
                });

            migrationBuilder.InsertData(
                table: "HermesUserContacts",
                columns: new[] { "ContactId", "HermesUserId" },
                values: new object[,]
                {
                    { new Guid("038da423-3391-4217-a21c-5b8ae593d52e"), new Guid("038da423-3391-4217-a21c-5b8ae593d52e") },
                    { new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07"), new Guid("038da423-3391-4217-a21c-5b8ae593d52e") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("038da423-3391-4217-a21c-5b8ae593d52e") },
                    { new Guid("d8793545-96c6-4901-8114-78d82c01504f"), new Guid("038da423-3391-4217-a21c-5b8ae593d52e") },
                    { new Guid("e48d16be-738d-473f-8505-fcd121d8ef35"), new Guid("038da423-3391-4217-a21c-5b8ae593d52e") },
                    { new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df"), new Guid("038da423-3391-4217-a21c-5b8ae593d52e") },
                    { new Guid("038da423-3391-4217-a21c-5b8ae593d52e"), new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07") },
                    { new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07"), new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07") },
                    { new Guid("d8793545-96c6-4901-8114-78d82c01504f"), new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07") },
                    { new Guid("e48d16be-738d-473f-8505-fcd121d8ef35"), new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07") },
                    { new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df"), new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07") },
                    { new Guid("038da423-3391-4217-a21c-5b8ae593d52e"), new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee") },
                    { new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07"), new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee") },
                    { new Guid("d8793545-96c6-4901-8114-78d82c01504f"), new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee") },
                    { new Guid("e48d16be-738d-473f-8505-fcd121d8ef35"), new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee") },
                    { new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df"), new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee") },
                    { new Guid("038da423-3391-4217-a21c-5b8ae593d52e"), new Guid("d8793545-96c6-4901-8114-78d82c01504f") },
                    { new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07"), new Guid("d8793545-96c6-4901-8114-78d82c01504f") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("d8793545-96c6-4901-8114-78d82c01504f") },
                    { new Guid("d8793545-96c6-4901-8114-78d82c01504f"), new Guid("d8793545-96c6-4901-8114-78d82c01504f") },
                    { new Guid("e48d16be-738d-473f-8505-fcd121d8ef35"), new Guid("d8793545-96c6-4901-8114-78d82c01504f") },
                    { new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df"), new Guid("d8793545-96c6-4901-8114-78d82c01504f") },
                    { new Guid("038da423-3391-4217-a21c-5b8ae593d52e"), new Guid("e48d16be-738d-473f-8505-fcd121d8ef35") },
                    { new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07"), new Guid("e48d16be-738d-473f-8505-fcd121d8ef35") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("e48d16be-738d-473f-8505-fcd121d8ef35") },
                    { new Guid("d8793545-96c6-4901-8114-78d82c01504f"), new Guid("e48d16be-738d-473f-8505-fcd121d8ef35") },
                    { new Guid("e48d16be-738d-473f-8505-fcd121d8ef35"), new Guid("e48d16be-738d-473f-8505-fcd121d8ef35") },
                    { new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df"), new Guid("e48d16be-738d-473f-8505-fcd121d8ef35") },
                    { new Guid("038da423-3391-4217-a21c-5b8ae593d52e"), new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df") },
                    { new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07"), new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df") },
                    { new Guid("d8793545-96c6-4901-8114-78d82c01504f"), new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df") },
                    { new Guid("e48d16be-738d-473f-8505-fcd121d8ef35"), new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df") },
                    { new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df"), new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df") }
                });

            migrationBuilder.InsertData(
                table: "HermesUserGroups",
                columns: new[] { "GroupId", "HermesUserId" },
                values: new object[,]
                {
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("038da423-3391-4217-a21c-5b8ae593d52e") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("2af2ada0-ad1f-4494-b038-db75d813fe07") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("d8793545-96c6-4901-8114-78d82c01504f") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("e48d16be-738d-473f-8505-fcd121d8ef35") },
                    { new Guid("4557069d-02ce-48bc-b728-72e5991dd3ee"), new Guid("eb975911-ba67-4365-b970-3e7fd1aab7df") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_HermesUserContacts_ContactId",
                table: "HermesUserContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_HermesUserGroups_GroupId",
                table: "HermesUserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "HermesUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "HermesUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HermesMessages");

            migrationBuilder.DropTable(
                name: "HermesUserContacts");

            migrationBuilder.DropTable(
                name: "HermesUserGroups");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "HermesUsers");
        }
    }
}
