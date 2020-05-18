using Microsoft.EntityFrameworkCore.Migrations;

namespace LatexReferences.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Styles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Styles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BibEntryFormats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    EntryType = table.Column<int>(nullable: false),
                    IncludeAddress = table.Column<bool>(nullable: false),
                    IncludeAnnote = table.Column<bool>(nullable: false),
                    IncludeAuthor = table.Column<bool>(nullable: false),
                    IncludeBooktitle = table.Column<bool>(nullable: false),
                    IncludeChapter = table.Column<bool>(nullable: false),
                    IncludeCrossReference = table.Column<bool>(nullable: false),
                    IncludeDOI = table.Column<bool>(nullable: false),
                    IncludeEdition = table.Column<bool>(nullable: false),
                    IncludeEditor = table.Column<bool>(nullable: false),
                    IncludeEmail = table.Column<bool>(nullable: false),
                    IncludeHowPublished = table.Column<bool>(nullable: false),
                    IncludeInstitution = table.Column<bool>(nullable: false),
                    IncludeJournal = table.Column<bool>(nullable: false),
                    IncludeKey = table.Column<bool>(nullable: false),
                    IncludeMonth = table.Column<bool>(nullable: false),
                    IncludeNumber = table.Column<bool>(nullable: false),
                    IncludeOrganization = table.Column<bool>(nullable: false),
                    IncludePages = table.Column<bool>(nullable: false),
                    IncludePublisher = table.Column<bool>(nullable: false),
                    IncludeSchool = table.Column<bool>(nullable: false),
                    IncludeSeries = table.Column<bool>(nullable: false),
                    IncludeTitle = table.Column<bool>(nullable: false),
                    IncludeType = table.Column<bool>(nullable: false),
                    IncludeVolume = table.Column<bool>(nullable: false),
                    IncludeYear = table.Column<bool>(nullable: false),
                    StyleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibEntryFormats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BibEntryFormats_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BibEntryFormats_StyleId",
                table: "BibEntryFormats",
                column: "StyleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BibEntryFormats");

            migrationBuilder.DropTable(
                name: "Styles");
        }
    }
}
