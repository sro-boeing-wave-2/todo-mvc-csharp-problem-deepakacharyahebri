using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoAssignment.Migrations
{
    public partial class changedNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckList_Notes_NotesId",
                table: "CheckList");

            migrationBuilder.DropForeignKey(
                name: "FK_Label_Notes_NotesId",
                table: "Label");

            migrationBuilder.RenameColumn(
                name: "NotesId",
                table: "Label",
                newName: "NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Label_NotesId",
                table: "Label",
                newName: "IX_Label_NoteId");

            migrationBuilder.RenameColumn(
                name: "NotesId",
                table: "CheckList",
                newName: "NoteId");

            migrationBuilder.RenameColumn(
                name: "ChickListStatus",
                table: "CheckList",
                newName: "CheckListStatus");

            migrationBuilder.RenameIndex(
                name: "IX_CheckList_NotesId",
                table: "CheckList",
                newName: "IX_CheckList_NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckList_Notes_NoteId",
                table: "CheckList",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Notes_NoteId",
                table: "Label",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckList_Notes_NoteId",
                table: "CheckList");

            migrationBuilder.DropForeignKey(
                name: "FK_Label_Notes_NoteId",
                table: "Label");

            migrationBuilder.RenameColumn(
                name: "NoteId",
                table: "Label",
                newName: "NotesId");

            migrationBuilder.RenameIndex(
                name: "IX_Label_NoteId",
                table: "Label",
                newName: "IX_Label_NotesId");

            migrationBuilder.RenameColumn(
                name: "NoteId",
                table: "CheckList",
                newName: "NotesId");

            migrationBuilder.RenameColumn(
                name: "CheckListStatus",
                table: "CheckList",
                newName: "ChickListStatus");

            migrationBuilder.RenameIndex(
                name: "IX_CheckList_NoteId",
                table: "CheckList",
                newName: "IX_CheckList_NotesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckList_Notes_NotesId",
                table: "CheckList",
                column: "NotesId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Notes_NotesId",
                table: "Label",
                column: "NotesId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
