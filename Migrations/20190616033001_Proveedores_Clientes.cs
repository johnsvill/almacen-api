using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventarioAPI.Migrations
{
    public partial class Proveedores_Clientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nit",
                table: "Facturas",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Nit = table.Column<string>(nullable: false),
                    Dpi = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Direccion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Nit);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    CodigoProveedor = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nit = table.Column<string>(nullable: false),
                    RazonSocial = table.Column<string>(nullable: true),
                    Direccion = table.Column<string>(nullable: true),
                    PaginaWeb = table.Column<string>(nullable: true),
                    ContactoPrincipal = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.CodigoProveedor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_Nit",
                table: "Facturas",
                column: "Nit");

            migrationBuilder.CreateIndex(
                name: "IX_EmailProveedor_CodigoProveedor",
                table: "EmailProveedor",
                column: "CodigoProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CodigoProveedor",
                table: "Compras",
                column: "CodigoProveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Proveedores_CodigoProveedor",
                table: "Compras",
                column: "CodigoProveedor",
                principalTable: "Proveedores",
                principalColumn: "CodigoProveedor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailProveedor_Proveedores_CodigoProveedor",
                table: "EmailProveedor",
                column: "CodigoProveedor",
                principalTable: "Proveedores",
                principalColumn: "CodigoProveedor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Clientes_Nit",
                table: "Facturas",
                column: "Nit",
                principalTable: "Clientes",
                principalColumn: "Nit",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Proveedores_CodigoProveedor",
                table: "Compras");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailProveedor_Proveedores_CodigoProveedor",
                table: "EmailProveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Clientes_Nit",
                table: "Facturas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_Nit",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_EmailProveedor_CodigoProveedor",
                table: "EmailProveedor");

            migrationBuilder.DropIndex(
                name: "IX_Compras_CodigoProveedor",
                table: "Compras");

            migrationBuilder.AlterColumn<string>(
                name: "Nit",
                table: "Facturas",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
