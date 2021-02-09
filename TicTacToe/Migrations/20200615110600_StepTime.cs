using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicTacToe.Migrations
{
    public partial class StepTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Games_gameId",
                table: "Steps");

            migrationBuilder.RenameColumn(
                name: "gameId",
                table: "Steps",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_gameId",
                table: "Steps",
                newName: "IX_Steps_GameId");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Steps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StepTime",
                table: "Steps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Games_GameId",
                table: "Steps",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Games_GameId",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "StepTime",
                table: "Steps");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Steps",
                newName: "gameId");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_GameId",
                table: "Steps",
                newName: "IX_Steps_gameId");

            migrationBuilder.AlterColumn<int>(
                name: "gameId",
                table: "Steps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Games_gameId",
                table: "Steps",
                column: "gameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
