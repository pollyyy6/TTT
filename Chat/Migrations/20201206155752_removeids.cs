using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Migrations
{
    public partial class removeids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetMessageId",
                schema: "Chat",
                table: "ForwardedMessages");

            migrationBuilder.AlterColumn<long>(
                name: "ChatMessageId",
                schema: "Chat",
                table: "ForwardedMessages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ChatMessageId",
                schema: "Chat",
                table: "ForwardedMessages",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "TargetMessageId",
                schema: "Chat",
                table: "ForwardedMessages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
