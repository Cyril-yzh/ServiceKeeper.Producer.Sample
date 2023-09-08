using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceKeeper.Producer.Sample.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TaskEntityV101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "TaskEntity",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "TaskEntity");
        }
    }
}
