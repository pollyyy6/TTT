using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Migrations
{
    public partial class invitatorid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitatorId",
                schema: "Chat",
                table: "Chats_Participants",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitatorId",
                schema: "Chat",
                table: "Chats_Participants");
        }
    }
}
