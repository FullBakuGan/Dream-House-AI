using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dream_House.Migrations
{
    /// <inheritdoc />
    public partial class FixUserDataAndAdUserCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ad_user_created",
                table: "ad_user_created");

            migrationBuilder.AlterTable(
                name: "ad_user_created",
                comment: "Хранит пользовательские объявления");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ad_user_created",
                type: "integer",
                nullable: false,
                comment: "Уникальный идентификатор объявления (автоинкремент)",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "ad_user_created",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Ссылка на пользователя (таблица users)");

            migrationBuilder.AddPrimaryKey(
                name: "ad_user_created_pkey",
                table: "ad_user_created",
                column: "id");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "date_of_birth", "email", "hash_password", "name", "phone_number", "RegistrationDate", "role_id", "surname" },
                values: new object[] { 100, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@example.com", "hashed_password", "Test", null, new DateTime(2025, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "User" });

            migrationBuilder.CreateIndex(
                name: "IX_ad_user_created_user_id",
                table: "ad_user_created",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user",
                table: "ad_user_created",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user",
                table: "ad_user_created");

            migrationBuilder.DropPrimaryKey(
                name: "ad_user_created_pkey",
                table: "ad_user_created");

            migrationBuilder.DropIndex(
                name: "IX_ad_user_created_user_id",
                table: "ad_user_created");

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 100);

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "ad_user_created");

            migrationBuilder.AlterTable(
                name: "ad_user_created",
                oldComment: "Хранит пользовательские объявления");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ad_user_created",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Уникальный идентификатор объявления (автоинкремент)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ad_user_created",
                table: "ad_user_created",
                column: "id");
        }
    }
}
