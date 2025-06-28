using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dream_House.Migrations
{
    /// <inheritdoc />
    public partial class newmigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adcreateviewmodel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adcreateviewmodel",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    area_obj = table.Column<decimal>(type: "numeric", nullable: true),
                    city_district_id = table.Column<int>(type: "integer", nullable: true),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    cost = table.Column<decimal>(type: "numeric", nullable: true),
                    count_of_rooms = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    district_id = table.Column<int>(type: "integer", nullable: true),
                    image_urls = table.Column<string>(type: "text", nullable: true),
                    microdistrict_id = table.Column<int>(type: "integer", nullable: true),
                    stage = table.Column<int>(type: "integer", nullable: false),
                    street_id = table.Column<int>(type: "integer", nullable: true),
                    type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adcreateviewmodel", x => x.id);
                });
        }
    }
}
