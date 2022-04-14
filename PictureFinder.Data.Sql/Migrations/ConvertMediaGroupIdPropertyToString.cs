using Microsoft.EntityFrameworkCore.Migrations;

namespace PictureFinder.Data.Sql.Migrations
{
    public partial class ConvertMediaGroupIdPropertyToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "MediaGroupId",
                "Photos",
                "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                "MediaGroupId",
                "Photos",
                "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}