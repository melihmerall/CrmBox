using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmBox.Persistance.Migrations.CrmBoxIdentity
{
    public partial class chatRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "ChatRooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "ChatRooms",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "ChatRooms");
        }
    }
}
