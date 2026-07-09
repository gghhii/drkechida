using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrKchida.Migrations
{
    /// <inheritdoc />
    public partial class AddTreatmentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "Treatments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullDescription",
                table: "Treatments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcedureSteps",
                table: "Treatments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "FullDescription",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "ProcedureSteps",
                table: "Treatments");
        }
    }
}
