using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PictureFinder.Data.Sql.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Photos",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Url = table.Column<string>("nvarchar(max)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Photos", x => x.Id); });

            migrationBuilder.CreateTable(
                "Tags",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Name = table.Column<string>("nvarchar(max)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Tags", x => x.Id); });

            migrationBuilder.CreateTable(
                "PhotoTag",
                table => new
                {
                    PhotosId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    TagsId = table.Column<Guid>("uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTag", x => new {x.PhotosId, x.TagsId});
                    table.ForeignKey(
                        "FK_PhotoTag_Photos_PhotosId",
                        x => x.PhotosId,
                        "Photos",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_PhotoTag_Tags_TagsId",
                        x => x.TagsId,
                        "Tags",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_PhotoTag_TagsId",
                "PhotoTag",
                "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PhotoTag");

            migrationBuilder.DropTable(
                "Photos");

            migrationBuilder.DropTable(
                "Tags");
        }
    }
}