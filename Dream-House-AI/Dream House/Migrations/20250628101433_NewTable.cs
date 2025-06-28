using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dream_House.Migrations
{
    /// <inheritdoc />
    public partial class NewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ad_user_created",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    district_id = table.Column<int>(type: "integer", nullable: true),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    city_district_id = table.Column<int>(type: "integer", nullable: true),
                    microdistrict_id = table.Column<int>(type: "integer", nullable: true),
                    street_id = table.Column<int>(type: "integer", nullable: true),
                    area_obj = table.Column<decimal>(type: "numeric", nullable: true),
                    cost = table.Column<decimal>(type: "numeric", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    count_of_rooms = table.Column<int>(type: "integer", nullable: false),
                    stage = table.Column<int>(type: "integer", nullable: false),
                    image_urls = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ad_user_created", x => x.id);
                    table.ForeignKey(
                        name: "FK_ad_user_created_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ad_user_created_city_district_city_district_id",
                        column: x => x.city_district_id,
                        principalTable: "city_district",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ad_user_created_district_district_id",
                        column: x => x.district_id,
                        principalTable: "district",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ad_user_created_microdistrict_microdistrict_id",
                        column: x => x.microdistrict_id,
                        principalTable: "microdistrict",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ad_user_created_street_street_id",
                        column: x => x.street_id,
                        principalTable: "street",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ad_user_created_type_obj_type_id",
                        column: x => x.type_id,
                        principalTable: "type_obj",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "adcreateviewmodel",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    district_id = table.Column<int>(type: "integer", nullable: true),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    city_district_id = table.Column<int>(type: "integer", nullable: true),
                    microdistrict_id = table.Column<int>(type: "integer", nullable: true),
                    street_id = table.Column<int>(type: "integer", nullable: true),
                    area_obj = table.Column<decimal>(type: "numeric", nullable: true),
                    cost = table.Column<decimal>(type: "numeric", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    count_of_rooms = table.Column<int>(type: "integer", nullable: false),
                    stage = table.Column<int>(type: "integer", nullable: false),
                    image_urls = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adcreateviewmodel", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ad_user_created_city_district_id",
                table: "ad_user_created",
                column: "city_district_id");

            migrationBuilder.CreateIndex(
                name: "IX_ad_user_created_city_id",
                table: "ad_user_created",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_ad_user_created_district_id",
                table: "ad_user_created",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_ad_user_created_microdistrict_id",
                table: "ad_user_created",
                column: "microdistrict_id");

            migrationBuilder.CreateIndex(
                name: "IX_ad_user_created_street_id",
                table: "ad_user_created",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "IX_ad_user_created_type_id",
                table: "ad_user_created",
                column: "type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ad_user_created");

            migrationBuilder.DropTable(
                name: "adcreateviewmodel");
        }
    }
}
