using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductToDbAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Author 1", "Description 1", "ISBN 1", 10.99, 9.9900000000000002, 7.9900000000000002, 8.9900000000000002, "Book 1" },
                    { 2, "Author 2", "Description 2", "ISBN 2", 20.989999999999998, 19.989999999999998, 17.989999999999998, 18.989999999999998, "Book 2" },
                    { 3, "Author 3", "Description 3", "ISBN 3", 30.989999999999998, 29.989999999999998, 27.989999999999998, 28.989999999999998, "Book 3" },
                    { 4, "Author 4", "Description 4", "ISBN 4", 40.990000000000002, 39.990000000000002, 37.990000000000002, 38.990000000000002, "Book 4" },
                    { 5, "Author 5", "Description 5", "ISBN 5", 50.990000000000002, 49.990000000000002, 47.990000000000002, 48.990000000000002, "Book 5" },
                    { 6, "Author 6", "Description 6", "ISBN 6", 60.990000000000002, 59.990000000000002, 57.990000000000002, 58.990000000000002, "Book 6" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
