using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrvalisProduction.Api.Data.Migrations;
[DbContext(typeof(ProductionDbContext))]
[Migration("202611050002_AlignColumnTypes")]
public sealed class AlignColumnTypes : Migration
{
    protected override void Up(MigrationBuilder m)
    {
        m.DropIndex(name: "IX_Production_Email_Date", table: "Production");
        m.AlterColumn<string>(name: "ProductDescription", table: "Product", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)", oldMaxLength: 50);
        m.AlterColumn<string>(name: "Image", table: "Product", type: "varchar(500)", unicode: false, maxLength: 500, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500);
        m.AlterColumn<string>(name: "MaterialDescription", table: "Material", type: "varchar(500)", unicode: false, maxLength: 500, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500);
        m.AlterColumn<string>(name: "Name", table: "User", type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(50)", oldMaxLength: 50);
        m.AlterColumn<DateTime>(name: "InitialDate", table: "User", type: "datetime", nullable: false, oldClrType: typeof(DateTime), oldType: "datetime2");
        m.AlterColumn<DateTime>(name: "EndDate", table: "User", type: "datetime", nullable: false, oldClrType: typeof(DateTime), oldType: "datetime2");
        m.AlterColumn<DateTime>(name: "Date", table: "Production", type: "datetime", nullable: false, oldClrType: typeof(DateTime), oldType: "datetime2");
        m.CreateIndex(name: "IX_Production_Email_Date", table: "Production", columns: new[] { "Email", "Date" });
    }
    protected override void Down(MigrationBuilder m)
    {
        m.DropIndex(name: "IX_Production_Email_Date", table: "Production");
        m.AlterColumn<string>(name: "ProductDescription", table: "Product", type: "nvarchar(50)", maxLength: 50, nullable: false, oldClrType: typeof(string), oldType: "varchar(50)", oldUnicode: false, oldMaxLength: 50);
        m.AlterColumn<string>(name: "Image", table: "Product", type: "nvarchar(500)", maxLength: 500, nullable: false, oldClrType: typeof(string), oldType: "varchar(500)", oldUnicode: false, oldMaxLength: 500);
        m.AlterColumn<string>(name: "MaterialDescription", table: "Material", type: "nvarchar(500)", maxLength: 500, nullable: false, oldClrType: typeof(string), oldType: "varchar(500)", oldUnicode: false, oldMaxLength: 500);
        m.AlterColumn<string>(name: "Name", table: "User", type: "nvarchar(50)", maxLength: 50, nullable: false, oldClrType: typeof(string), oldType: "varchar(50)", oldUnicode: false, oldMaxLength: 50);
        m.AlterColumn<DateTime>(name: "InitialDate", table: "User", type: "datetime2", nullable: false, oldClrType: typeof(DateTime), oldType: "datetime");
        m.AlterColumn<DateTime>(name: "EndDate", table: "User", type: "datetime2", nullable: false, oldClrType: typeof(DateTime), oldType: "datetime");
        m.AlterColumn<DateTime>(name: "Date", table: "Production", type: "datetime2", nullable: false, oldClrType: typeof(DateTime), oldType: "datetime");
        m.CreateIndex(name: "IX_Production_Email_Date", table: "Production", columns: new[] { "Email", "Date" });
    }
}
