using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApplication.Web.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_TodoLists_ListId",
                table: "TodoTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_TodoLists_ListId",
                table: "TodoTasks",
                column: "ListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_TodoLists_ListId",
                table: "TodoTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_TodoLists_ListId",
                table: "TodoTasks",
                column: "ListId",
                principalTable: "TodoLists",
                principalColumn: "Id");
        }
    }
}
