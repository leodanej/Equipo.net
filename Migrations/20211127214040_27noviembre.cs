using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bakend.Migrations
{
    public partial class _27noviembre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GLId",
                table: "puntos",
                newName: "LugarId");

            migrationBuilder.RenameIndex(
                name: "IX_puntos_GLId",
                table: "puntos",
                newName: "IX_puntos_LugarId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FFin",
                table: "Usu_Rec",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LugarId",
                table: "puntos",
                newName: "GLId");

            migrationBuilder.RenameIndex(
                name: "IX_puntos_LugarId",
                table: "puntos",
                newName: "IX_puntos_GLId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FFin",
                table: "Usu_Rec",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
