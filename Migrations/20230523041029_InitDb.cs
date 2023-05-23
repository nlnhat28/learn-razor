using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using RAZOR_EF.Models;

#nullable disable

namespace RAZOR_EF.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                });
            // Insert
            /*
            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Title", "CreatedTime", "Content" },
                values: new object[] {
                    "Nguyễn Thị Phương Linh",
                    new DateTime(2022,8,1),
                    "2000 - HR Rikkeisoft - FTU - Đô Lương"
                });
            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Title", "CreatedTime", "Content" },
                values: new object[] {
                    "Nguyễn Thị Thu Hoài",
                    new DateTime(2022,10,5),
                    "2001 - HR DCV - HOU - Lục Nam"
                });
            */

            // Use Bogus
            Randomizer.Seed = new Random(8675309);
            var fake = new Faker<Article>("en");
            fake.RuleFor(a => a.Title, f => f.Lorem.Sentence(5, 5));
            fake.RuleFor(a => a.CreatedTime, f => f.Date.Between(new DateTime(2000,1,1), DateTime.Now));
            fake.RuleFor(a => a.Content, f => f.Lorem.Paragraph(10));

            for (int i = 0; i < 150; i++)
            {
                var article = fake.Generate();
                migrationBuilder.InsertData(
                    table: "Article",
                    columns: new[] { "Title", "CreatedTime", "Content" },
                    values: new object[]{
                        article.Title,
                        article.CreatedTime,
                        article.Content
                    }
                );
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");
        }
    }
}
