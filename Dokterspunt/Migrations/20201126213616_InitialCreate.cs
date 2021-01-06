using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dokterspunt.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Afspraaktype",
                columns: table => new
                {
                    AfspraakTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoortAfspraak = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afspraaktype", x => x.AfspraakTypeID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Klacht",
                columns: table => new
                {
                    KlachtID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klacht", x => x.KlachtID);
                });

            migrationBuilder.CreateTable(
                name: "Praktijk",
                columns: table => new
                {
                    PraktijkID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gemeente = table.Column<string>(nullable: true),
                    Straat = table.Column<string>(nullable: true),
                    HuisNr = table.Column<string>(maxLength: 20, nullable: true),
                    Postcode = table.Column<int>(nullable: false),
                    Breedtegraad = table.Column<decimal>(type: "decimal(30,20)", nullable: false),
                    Lengtegraad = table.Column<decimal>(type: "decimal(30,20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Praktijk", x => x.PraktijkID);
                });

            migrationBuilder.CreateTable(
                name: "Specialisatie",
                columns: table => new
                {
                    SpecialisatieID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialisatie", x => x.SpecialisatieID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patiënt",
                columns: table => new
                {
                    PatiëntID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(nullable: true),
                    Achternaam = table.Column<string>(nullable: true),
                    Gemeente = table.Column<string>(nullable: true),
                    Straat = table.Column<string>(nullable: true),
                    HuisNr = table.Column<string>(maxLength: 20, nullable: true),
                    Postcode = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patiënt", x => x.PatiëntID);
                    table.ForeignKey(
                        name: "FK_Patiënt_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dokter",
                columns: table => new
                {
                    DokterID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(nullable: true),
                    Achternaam = table.Column<string>(nullable: true),
                    SpecialisatieID = table.Column<int>(nullable: false),
                    PraktijkID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dokter", x => x.DokterID);
                    table.ForeignKey(
                        name: "FK_Dokter_Praktijk_PraktijkID",
                        column: x => x.PraktijkID,
                        principalTable: "Praktijk",
                        principalColumn: "PraktijkID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dokter_Specialisatie_SpecialisatieID",
                        column: x => x.SpecialisatieID,
                        principalTable: "Specialisatie",
                        principalColumn: "SpecialisatieID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dokter_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KlachtPatiënt",
                columns: table => new
                {
                    KlachtPatiëntID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatiëntID = table.Column<int>(nullable: false),
                    KlachtID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KlachtPatiënt", x => x.KlachtPatiëntID);
                    table.ForeignKey(
                        name: "FK_KlachtPatiënt_Klacht_KlachtID",
                        column: x => x.KlachtID,
                        principalTable: "Klacht",
                        principalColumn: "KlachtID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KlachtPatiënt_Patiënt_PatiëntID",
                        column: x => x.PatiëntID,
                        principalTable: "Patiënt",
                        principalColumn: "PatiëntID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedischDossier",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnose = table.Column<string>(nullable: true),
                    MedischVerleden = table.Column<string>(nullable: true),
                    Medicatie = table.Column<string>(nullable: true),
                    PatiëntID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedischDossier", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MedischDossier_Patiënt_PatiëntID",
                        column: x => x.PatiëntID,
                        principalTable: "Patiënt",
                        principalColumn: "PatiëntID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Afspraak",
                columns: table => new
                {
                    AfspraakID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatiëntID = table.Column<int>(nullable: false),
                    DokterID = table.Column<int>(nullable: false),
                    AfspraakTypeID = table.Column<int>(nullable: false),
                    AfspraakMoment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afspraak", x => x.AfspraakID);
                    table.ForeignKey(
                        name: "FK_Afspraak_Afspraaktype_AfspraakTypeID",
                        column: x => x.AfspraakTypeID,
                        principalTable: "Afspraaktype",
                        principalColumn: "AfspraakTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Afspraak_Dokter_DokterID",
                        column: x => x.DokterID,
                        principalTable: "Dokter",
                        principalColumn: "DokterID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Afspraak_Patiënt_PatiëntID",
                        column: x => x.PatiëntID,
                        principalTable: "Patiënt",
                        principalColumn: "PatiëntID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Afspraak_AfspraakTypeID",
                table: "Afspraak",
                column: "AfspraakTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Afspraak_DokterID",
                table: "Afspraak",
                column: "DokterID");

            migrationBuilder.CreateIndex(
                name: "IX_Afspraak_PatiëntID",
                table: "Afspraak",
                column: "PatiëntID");

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
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Dokter_PraktijkID",
                table: "Dokter",
                column: "PraktijkID");

            migrationBuilder.CreateIndex(
                name: "IX_Dokter_SpecialisatieID",
                table: "Dokter",
                column: "SpecialisatieID");

            migrationBuilder.CreateIndex(
                name: "IX_Dokter_UserID",
                table: "Dokter",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KlachtPatiënt_KlachtID",
                table: "KlachtPatiënt",
                column: "KlachtID");

            migrationBuilder.CreateIndex(
                name: "IX_KlachtPatiënt_PatiëntID",
                table: "KlachtPatiënt",
                column: "PatiëntID");

            migrationBuilder.CreateIndex(
                name: "IX_MedischDossier_PatiëntID",
                table: "MedischDossier",
                column: "PatiëntID");

            migrationBuilder.CreateIndex(
                name: "IX_Patiënt_UserID",
                table: "Patiënt",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Afspraak");

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
                name: "KlachtPatiënt");

            migrationBuilder.DropTable(
                name: "MedischDossier");

            migrationBuilder.DropTable(
                name: "Afspraaktype");

            migrationBuilder.DropTable(
                name: "Dokter");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Klacht");

            migrationBuilder.DropTable(
                name: "Patiënt");

            migrationBuilder.DropTable(
                name: "Praktijk");

            migrationBuilder.DropTable(
                name: "Specialisatie");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
