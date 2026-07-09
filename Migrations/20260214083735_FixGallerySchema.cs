using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrKchida.Migrations
{
    /// <inheritdoc />
    public partial class FixGallerySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The column CategoryDisplay was incorrectly added in a previous migration/manual db change but is not in the model.
            // We need to drop it to fix the NOT NULL constraint error.
            migrationBuilder.DropColumn(
                name: "CategoryDisplay",
                table: "GalleryItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryDisplay",
                table: "GalleryItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
