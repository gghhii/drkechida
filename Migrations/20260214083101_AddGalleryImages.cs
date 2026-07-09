using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrKchida.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only add the new columns to the existing GalleryItems table
            migrationBuilder.AddColumn<string>(
                name: "ImageBefore",
                table: "GalleryItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "images/gallery/default-before.jpg");

            migrationBuilder.AddColumn<string>(
                name: "ImageAfter",
                table: "GalleryItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "images/gallery/default-after.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBefore",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "ImageAfter",
                table: "GalleryItems");
        }
    }
}
