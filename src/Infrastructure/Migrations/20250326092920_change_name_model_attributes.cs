using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookPro.Infrastructure.Migrations
{
    public partial class change_name_model_attributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Services_ServiceId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.CreateTable(
                name: "TypeServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeServices", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_TypeServices_ServiceId",
                table: "Companies",
                column: "ServiceId",
                principalTable: "TypeServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_TypeServices_ServiceId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "TypeServices");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameService = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Services_ServiceId",
                table: "Companies",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
