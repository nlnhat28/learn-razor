using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using RAZOR_EF.Models;

#nullable disable

namespace RAZOR_EF.Migrations
{
    public partial class InsertDataArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use Bogus
            Randomizer.Seed = new Random(8675309);
            var fake = new Faker<Article>();
            var lorem = new Bogus.DataSets.Lorem(locale: "fr_CA");
            fake.RuleFor(a => a.Title, f => lorem.Sentence(5, 10));
            fake.RuleFor(a => a.CreatedTime, f => f.Date.Between(new DateTime(2000, 1, 1), DateTime.Now));
            fake.RuleFor(a => a.Content, f => lorem.Paragraph(10));

            for (int i = 0; i < 102; i++)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
