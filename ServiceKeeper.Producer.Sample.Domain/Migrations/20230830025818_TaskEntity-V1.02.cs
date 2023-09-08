using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceKeeper.Producer.Sample.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TaskEntityV102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "TaskEntity",
                newName: "PublishKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishKey",
                table: "TaskEntity",
                newName: "Key");
        }
    }
}
