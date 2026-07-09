using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrKchida.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToTreatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Treatments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Treatments");
        }
    }
}
