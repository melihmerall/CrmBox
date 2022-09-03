using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmBox.Persistance.Migrations.CrmBoxIdentity
{
    public partial class ChatRoomAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoom");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "MessageTime",
                table: "Messages",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "MessageText",
                table: "Messages",
                newName: "Content");

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

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToRoomId",
                table: "Messages",
                column: "ToRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AdminId",
                table: "Rooms",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_ToRoomId",
                table: "Messages",
                column: "ToRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_ToRoomId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ToRoomId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ToRoomId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Messages",
                newName: "MessageTime");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Messages",
                newName: "MessageText");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ChatRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoom", x => x.Id);
                });
        }
    }
}
