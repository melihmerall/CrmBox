using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmBox.Persistance.Migrations.CrmBoxIdentity
{
    public partial class SignalRAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_AppUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_AppUserId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "OwnerConnectionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SentDT",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Rooms",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_AppUserId",
                table: "Rooms",
                newName: "IX_Rooms_AdminId");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Messages",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Messages",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Messages",
                newName: "FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_AppUserId",
                table: "Messages",
                newName: "IX_Messages_FromUserId");

            migrationBuilder.AddColumn<int>(
                name: "ToRoomId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToRoomId",
                table: "Messages",
                column: "ToRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_FromUserId",
                table: "Messages",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_ToRoomId",
                table: "Messages",
                column: "ToRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_AdminId",
                table: "Rooms",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
