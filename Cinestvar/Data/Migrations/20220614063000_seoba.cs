using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinestvar.Data.Migrations
{
    public partial class seoba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    IdFilma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivFilma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trajanje = table.Column<int>(type: "int", nullable: false),
                    Zanr = table.Column<int>(type: "int", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecenzijaLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CijenaKarte = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.IdFilma);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaSjedista",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRezervacije = table.Column<int>(type: "int", nullable: false),
                    OznakaSjedista = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaSjedista", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sala",
                columns: table => new
                {
                    IdSale = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivSale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrojRedova = table.Column<int>(type: "int", nullable: false),
                    BrojKolona = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sala", x => x.IdSale);
                });

            migrationBuilder.CreateTable(
                name: "Stavka",
                columns: table => new
                {
                    IdStavke = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivStavke = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpisStavke = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipStavke = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stavka", x => x.IdStavke);
                });

            migrationBuilder.CreateTable(
                name: "Termin",
                columns: table => new
                {
                    IdTermina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PocetakTermina = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KrajTermina = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdFilma = table.Column<int>(type: "int", nullable: false),
                    IdSale = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termin", x => x.IdTermina);
                    table.ForeignKey(
                        name: "FK_Termin_Film_IdFilma",
                        column: x => x.IdFilma,
                        principalTable: "Film",
                        principalColumn: "IdFilma",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Termin_Sala_IdSale",
                        column: x => x.IdSale,
                        principalTable: "Sala",
                        principalColumn: "IdSale",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    IdRezervacije = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTermina = table.Column<int>(type: "int", nullable: false),
                    IdKorisnika = table.Column<int>(type: "int", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.IdRezervacije);
                    table.ForeignKey(
                        name: "FK_Rezervacija_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervacija_Termin_IdTermina",
                        column: x => x.IdTermina,
                        principalTable: "Termin",
                        principalColumn: "IdTermina",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_IdentityUserId",
                table: "Rezervacija",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_IdTermina",
                table: "Rezervacija",
                column: "IdTermina");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_IdFilma",
                table: "Termin",
                column: "IdFilma");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_IdSale",
                table: "Termin",
                column: "IdSale");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "RezervacijaSjedista");

            migrationBuilder.DropTable(
                name: "Stavka");

            migrationBuilder.DropTable(
                name: "Termin");

            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "Sala");
        }
    }
}
