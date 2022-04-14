using Microsoft.EntityFrameworkCore.Migrations;

namespace PictureFinder.Data.Sql.Migrations
{
    public partial class AddMediaGroupIdIndexForPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Name",
                "Tags",
                "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                "Url",
                "Photos",
                "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                "MediaGroupId",
                "Photos",
                "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                "IX_Photos_MediaGroupId",
                "Photos",
                "MediaGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "IX_Photos_MediaGroupId",
                "Photos");

            migrationBuilder.DropColumn(
                "MediaGroupId",
                "Photos");

            migrationBuilder.AlterColumn<string>(
                "Name",
                "Tags",
                "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "Url",
                "Photos",
                "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}