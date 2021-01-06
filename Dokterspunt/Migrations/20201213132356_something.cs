using Microsoft.EntityFrameworkCore.Migrations;

namespace Dokterspunt.Migrations
{
    public partial class something : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "Specialisatie",
                newName: "Specialisatie",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "Praktijk",
                newName: "Praktijk",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "Patiënt",
                newName: "Patiënt",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "MedischDossier",
                newName: "MedischDossier",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "KlachtPatiënt",
                newName: "KlachtPatiënt",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "Klacht",
                newName: "Klacht",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "Dokter",
                newName: "Dokter",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "Afspraaktype",
                newName: "Afspraaktype",
                newSchema: "WEBAPPS");

            migrationBuilder.RenameTable(
                name: "Afspraak",
                newName: "Afspraak",
                newSchema: "WEBAPPS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Specialisatie",
                schema: "WEBAPPS",
                newName: "Specialisatie");

            migrationBuilder.RenameTable(
                name: "Praktijk",
                schema: "WEBAPPS",
                newName: "Praktijk");

            migrationBuilder.RenameTable(
                name: "Patiënt",
                schema: "WEBAPPS",
                newName: "Patiënt");

            migrationBuilder.RenameTable(
                name: "MedischDossier",
                schema: "WEBAPPS",
                newName: "MedischDossier");

            migrationBuilder.RenameTable(
                name: "KlachtPatiënt",
                schema: "WEBAPPS",
                newName: "KlachtPatiënt");

            migrationBuilder.RenameTable(
                name: "Klacht",
                schema: "WEBAPPS",
                newName: "Klacht");

            migrationBuilder.RenameTable(
                name: "Dokter",
                schema: "WEBAPPS",
                newName: "Dokter");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "WEBAPPS",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "WEBAPPS",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "WEBAPPS",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "WEBAPPS",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "WEBAPPS",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "WEBAPPS",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "WEBAPPS",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Afspraaktype",
                schema: "WEBAPPS",
                newName: "Afspraaktype");

            migrationBuilder.RenameTable(
                name: "Afspraak",
                schema: "WEBAPPS",
                newName: "Afspraak");
        }
    }
}
