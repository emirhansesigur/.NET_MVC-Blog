using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminBlog.Migrations
{
    public partial class mig_2addcategryname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Blog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Blog");
        }
    }
}
