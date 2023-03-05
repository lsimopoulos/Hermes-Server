using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), 0, "6cc8fc94-c3e2-4ae0-8feb-e5d321a5b508", "testGroup@haha.com", false, true, false, null, "testGroup", null, null, null, null, false, "73db1892-5227-4455-81d3-c146329c9ec3", false, "testGroup@haha.com" },
                    { new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb"), 0, "a76e0f8d-e84e-4918-858f-47a0a80a4c67", null, false, false, false, null, "haha4", null, "haha4@haha.com", "AQAAAAIAAYagAAAAEEZEmLZpDkTtKY7kDBJ+5r8omEXGGVZNRBKSBtnRG6y9bXPVFN5d9g4ATbwyhh7zIw==", null, false, "66d5570b-9283-47a0-ac5e-b896744aa521", false, "haha4@haha.com" },
                    { new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f"), 0, "93a20090-a678-43d0-86b4-c5da1411a8eb", null, false, false, false, null, "haha1", null, "haha1@haha.com", "AQAAAAIAAYagAAAAEDdR4tn/pMvITt+stWbakoNEdvUKstzJq+j1WYGEKUt1fXdVDFrgp0dRHm8IelMgzw==", null, false, "7d59f64f-68d7-4e2a-ba14-d18dce4b3329", false, "haha1@haha.com" },
                    { new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb"), 0, "fd7d3e8d-2be8-49c0-bfaf-3155208dbfa8", null, false, false, false, null, "haha2", null, "haha2@haha.com", "AQAAAAIAAYagAAAAENbcIPoM6k/Uv+wzWwU0K7B9WrXMzmWR4IepPgGNJB2m7qpZiAEfuNSyjd6guTFfyA==", null, false, "c4ac2d69-4701-4692-81c0-a09077a5ed7a", false, "haha2@haha.com" },
                    { new Guid("ea398905-831c-444b-b48f-7072dbb506c9"), 0, "0641635e-58b1-4106-83e9-a12de1814067", null, false, false, false, null, "haha3", null, "haha3@haha.com", "AQAAAAIAAYagAAAAEFer1oLsQqsK916m4sOtMxtDLXyplNAKjO/ufjZiv3K2HXmBhHkAwj9phG4DbeItNA==", null, false, "8a2265e9-4ced-480f-b200-c570a4811ba8", false, "haha3@haha.com" },
                    { new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa"), 0, "39781d67-b70e-4265-8269-e3efbf59b1ed", null, false, false, false, null, "haha0", null, "haha0@haha.com", "AQAAAAIAAYagAAAAEPPr4tGT4Z1GgopMNI5dphnFft5oTEknVmmUwfoc1Ya2l+0Yd6KpDYncOi5yrHNQog==", null, false, "6fc3c223-0273-4f85-b044-1e713346b971", false, "haha0@haha.com" }
                });

            migrationBuilder.InsertData(
                table: "HermesUserContacts",
                columns: new[] { "ContactId", "HermesUserId" },
                values: new object[,]
                {
                    { new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb"), new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea") },
                    { new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f"), new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea") },
                    { new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb"), new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea") },
                    { new Guid("ea398905-831c-444b-b48f-7072dbb506c9"), new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea") },
                    { new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa"), new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb") },
                    { new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb"), new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb") },
                    { new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f"), new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb") },
                    { new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb"), new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb") },
                    { new Guid("ea398905-831c-444b-b48f-7072dbb506c9"), new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb") },
                    { new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa"), new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f") },
                    { new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb"), new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f") },
                    { new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f"), new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f") },
                    { new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb"), new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f") },
                    { new Guid("ea398905-831c-444b-b48f-7072dbb506c9"), new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f") },
                    { new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa"), new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb") },
                    { new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb"), new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb") },
                    { new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f"), new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb") },
                    { new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb"), new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb") },
                    { new Guid("ea398905-831c-444b-b48f-7072dbb506c9"), new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb") },
                    { new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa"), new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("ea398905-831c-444b-b48f-7072dbb506c9") },
                    { new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb"), new Guid("ea398905-831c-444b-b48f-7072dbb506c9") },
                    { new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f"), new Guid("ea398905-831c-444b-b48f-7072dbb506c9") },
                    { new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb"), new Guid("ea398905-831c-444b-b48f-7072dbb506c9") },
                    { new Guid("ea398905-831c-444b-b48f-7072dbb506c9"), new Guid("ea398905-831c-444b-b48f-7072dbb506c9") },
                    { new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa"), new Guid("ea398905-831c-444b-b48f-7072dbb506c9") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa") },
                    { new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb"), new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa") },
                    { new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f"), new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa") },
                    { new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb"), new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa") },
                    { new Guid("ea398905-831c-444b-b48f-7072dbb506c9"), new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa") },
                    { new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa"), new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa") }
                });

            migrationBuilder.InsertData(
                table: "HermesUserGroups",
                columns: new[] { "GroupId", "HermesUserId" },
                values: new object[,]
                {
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("06e5492e-bcc8-4b86-9922-04b11349aadb") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("7ed880b2-2fb6-464d-af5d-1dd2f1dec54f") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("cd8dbff0-cbfe-4f6c-921c-06abb41968fb") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("ea398905-831c-444b-b48f-7072dbb506c9") },
                    { new Guid("0012d980-ef0b-4b95-80f6-3d7079f2c7ea"), new Guid("f1ad9e71-adfa-475c-8945-0c24795624aa") }
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
