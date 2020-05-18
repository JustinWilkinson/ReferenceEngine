﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace LatexReferences.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BibliographyStyles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibliographyStyles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntryStyles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    BibliographyStyleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryStyles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryStyles_BibliographyStyles_BibliographyStyleId",
                        column: x => x.BibliographyStyleId,
                        principalTable: "BibliographyStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntryStyles_BibliographyStyleId",
                table: "EntryStyles",
                column: "BibliographyStyleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryStyles");

            migrationBuilder.DropTable(
                name: "BibliographyStyles");
        }
    }
}