using Microsoft.EntityFrameworkCore.Migrations;

namespace RazorPagesMovie.Migrations
{
    public partial class Rating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Movie");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Movie",
                newName: "TG_addr");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Movie",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extra",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileUrl",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Extra",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "ProfileUrl",
                table: "Movie");

            migrationBuilder.RenameColumn(
                name: "TG_addr",
                table: "Movie",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Movie",
                newName: "Genre");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Movie",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
