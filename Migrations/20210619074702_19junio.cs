using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bakend.Migrations
{
    public partial class _19junio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntradasSalidas",
                columns: table => new
                {
                    EntSalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoId = table.Column<int>(type: "int", nullable: false),
                    GLId = table.Column<int>(type: "int", nullable: false),
                    Entrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasSalidas", x => x.EntSalId);
                });

            migrationBuilder.CreateTable(
                name: "geolugares",
                columns: table => new
                {
                    GLId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LugId = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geolugares", x => x.GLId);
                });

            migrationBuilder.CreateTable(
                name: "Lugar",
                columns: table => new
                {
                    LugarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    domicilio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitud = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    longitud = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lugar", x => x.LugarId);
                });

            migrationBuilder.CreateTable(
                name: "posiciones",
                columns: table => new
                {
                    PosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoId = table.Column<int>(type: "int", nullable: false),
                    latitud = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    longitud = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    FStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posiciones", x => x.PosId);
                });

            migrationBuilder.CreateTable(
                name: "puntos",
                columns: table => new
                {
                    PuntoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GLId = table.Column<int>(type: "int", nullable: false),
                    latitud = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    longitud = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_puntos", x => x.PuntoId);
                });

            migrationBuilder.CreateTable(
                name: "Recurso",
                columns: table => new
                {
                    RecursoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recurso", x => x.RecursoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Usuarioid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    correo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Usuarioid);
                });

            migrationBuilder.CreateTable(
                name: "Usu_Rec",
                columns: table => new
                {
                    MovId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    RecursoId = table.Column<int>(type: "int", nullable: false),
                    FInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usu_Rec", x => x.MovId);
                    table.ForeignKey(
                        name: "FK_Usu_Rec_Recurso_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "Recurso",
                        principalColumn: "RecursoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usu_Rec_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_puntos_GLId",
                table: "puntos",
                column: "GLId");

            migrationBuilder.CreateIndex(
                name: "IX_Usu_Rec_RecursoId",
                table: "Usu_Rec",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usu_Rec_UsuarioId",
                table: "Usu_Rec",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_correo",
                table: "Usuario",
                column: "correo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntradasSalidas");

            migrationBuilder.DropTable(
                name: "geolugares");

            migrationBuilder.DropTable(
                name: "Lugar");

            migrationBuilder.DropTable(
                name: "posiciones");

            migrationBuilder.DropTable(
                name: "puntos");

            migrationBuilder.DropTable(
                name: "Usu_Rec");

            migrationBuilder.DropTable(
                name: "Recurso");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
