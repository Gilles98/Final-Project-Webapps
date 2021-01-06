using Microsoft.EntityFrameworkCore.Migrations;

namespace Dokterspunt.Migrations
{
    public partial class something : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Webapps");

            migrationBuilder.RenameTable(
                name: "Specialisatie",
                newName: "Specialisatie",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "Praktijk",
                newName: "Praktijk",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "Patiënt",
                newName: "Patiënt",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "MedischDossier",
                newName: "MedischDossier",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "KlachtPatiënt",
                newName: "KlachtPatiënt",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "Klacht",
                newName: "Klacht",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "Dokter",
                newName: "Dokter",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "Afspraaktype",
                newName: "Afspraaktype",
                newSchema: "Webapps");

            migrationBuilder.RenameTable(
                name: "Afspraak",
                newName: "Afspraak",
                newSchema: "Webapps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Specialisatie",
                schema: "Webapps",
                newName: "Specialisatie");

            migrationBuilder.RenameTable(
                name: "Praktijk",
                schema: "Webapps",
                newName: "Praktijk");

            migrationBuilder.RenameTable(
                name: "Patiënt",
                schema: "Webapps",
                newName: "Patiënt");

            migrationBuilder.RenameTable(
                name: "MedischDossier",
                schema: "Webapps",
                newName: "MedischDossier");

            migrationBuilder.RenameTable(
                name: "KlachtPatiënt",
                schema: "Webapps",
                newName: "KlachtPatiënt");

            migrationBuilder.RenameTable(
                name: "Klacht",
                schema: "Webapps",
                newName: "Klacht");

            migrationBuilder.RenameTable(
                name: "Dokter",
                schema: "Webapps",
                newName: "Dokter");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "Webapps",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "Webapps",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "Webapps",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "Webapps",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "Webapps",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "Webapps",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "Webapps",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Afspraaktype",
                schema: "Webapps",
                newName: "Afspraaktype");

            migrationBuilder.RenameTable(
                name: "Afspraak",
                schema: "Webapps",
                newName: "Afspraak");
        }
    }
}
