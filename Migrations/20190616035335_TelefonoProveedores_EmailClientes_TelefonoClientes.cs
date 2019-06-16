using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventarioAPI.Migrations
{
    public partial class TelefonoProveedores_EmailClientes_TelefonoClientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailClientes",
                columns: table => new
                {
                    CodigoEmail = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Nit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailClientes", x => x.CodigoEmail);
                    table.ForeignKey(
                        name: "FK_EmailClientes_Clientes_Nit",
                        column: x => x.Nit,
                        principalTable: "Clientes",
                        principalColumn: "Nit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TelefonoClientes",
                columns: table => new
                {
                    CodigoTelefono = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Numero = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    Nit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonoClientes", x => x.CodigoTelefono);
                    table.ForeignKey(
                        name: "FK_TelefonoClientes_Clientes_Nit",
                        column: x => x.Nit,
                        principalTable: "Clientes",
                        principalColumn: "Nit",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TelefonoProveedores",
                columns: table => new
                {
                    CodigoTelefono = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Numero = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    CodigoProveedor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonoProveedores", x => x.CodigoTelefono);
                    table.ForeignKey(
                        name: "FK_TelefonoProveedores_Proveedores_CodigoProveedor",
                        column: x => x.CodigoProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "CodigoProveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailClientes_Nit",
                table: "EmailClientes",
                column: "Nit");

            migrationBuilder.CreateIndex(
                name: "IX_TelefonoClientes_Nit",
                table: "TelefonoClientes",
                column: "Nit");

            migrationBuilder.CreateIndex(
                name: "IX_TelefonoProveedores_CodigoProveedor",
                table: "TelefonoProveedores",
                column: "CodigoProveedor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailClientes");

            migrationBuilder.DropTable(
                name: "TelefonoClientes");

            migrationBuilder.DropTable(
                name: "TelefonoProveedores");
        }
    }
}
