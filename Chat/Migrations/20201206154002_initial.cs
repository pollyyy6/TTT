using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Chat");

            migrationBuilder.CreateTable(
                name: "Chats",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "Chat",
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats_Participants",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Participants_Chats_ChatId",
                        column: x => x.ChatId,
                        principalSchema: "Chat",
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForwardedMessages",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForwardedMessageId = table.Column<long>(type: "bigint", nullable: false),
                    TargetMessageId = table.Column<long>(type: "bigint", nullable: false),
                    ChatMessageId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForwardedMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForwardedMessages_ChatMessages_ChatMessageId",
                        column: x => x.ChatMessageId,
                        principalSchema: "Chat",
                        principalTable: "ChatMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ForwardedMessages_ChatMessages_ForwardedMessageId",
                        column: x => x.ForwardedMessageId,
                        principalSchema: "Chat",
                        principalTable: "ChatMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatId",
                schema: "Chat",
                table: "ChatMessages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Participants_ChatId",
                schema: "Chat",
                table: "Chats_Participants",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ForwardedMessages_ChatMessageId",
                schema: "Chat",
                table: "ForwardedMessages",
                column: "ChatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ForwardedMessages_ForwardedMessageId",
                schema: "Chat",
                table: "ForwardedMessages",
                column: "ForwardedMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats_Participants",
                schema: "Chat");

            migrationBuilder.DropTable(
                name: "ForwardedMessages",
                schema: "Chat");

            migrationBuilder.DropTable(
                name: "ChatMessages",
                schema: "Chat");

            migrationBuilder.DropTable(
                name: "Chats",
                schema: "Chat");
        }
    }
}
