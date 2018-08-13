using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoAssignment.Migrations
{
    public partial class addedlabelsandclists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_Notes_NoteId",
                table: "Label");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Label",
                table: "Label");

            migrationBuilder.RenameTable(
                name: "Label",
                newName: "Labels");

            migrationBuilder.RenameIndex(
                name: "IX_Label_NoteId",
                table: "Labels",
                newName: "IX_Labels_NoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Labels",
                table: "Labels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Notes_NoteId",
                table: "Labels",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Notes_NoteId",
                table: "Labels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Labels",
                table: "Labels");

            migrationBuilder.RenameTable(
                name: "Labels",
                newName: "Label");

            migrationBuilder.RenameIndex(
                name: "IX_Labels_NoteId",
                table: "Label",
                newName: "IX_Label_NoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Label",
                table: "Label",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Notes_NoteId",
                table: "Label",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
