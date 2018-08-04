using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoAssignmentSimple.Migrations
{
    public partial class addedondeletecascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckList_Note_NoteId",
                table: "CheckList");

            migrationBuilder.DropForeignKey(
                name: "FK_Label_Note_NoteId",
                table: "Label");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckList_Note_NoteId",
                table: "CheckList",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Note_NoteId",
                table: "Label",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckList_Note_NoteId",
                table: "CheckList");

            migrationBuilder.DropForeignKey(
                name: "FK_Label_Note_NoteId",
                table: "Label");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckList_Note_NoteId",
                table: "CheckList",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Note_NoteId",
                table: "Label",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
