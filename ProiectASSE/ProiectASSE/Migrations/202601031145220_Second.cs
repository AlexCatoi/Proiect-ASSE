// <copyright file="202601031145220_Second.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            this.DropForeignKey("dbo.Categories", "Book_Id", "dbo.Books");
            this.DropForeignKey("dbo.BookCopies", "Rent_Id", "dbo.Rents");
            this.DropIndex("dbo.Categories", new[] { "Book_Id" });
            this.DropIndex("dbo.BookCopies", new[] { "Rent_Id" });
            this.CreateTable(
                "dbo.CategoryBooks",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Book_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Book_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.Book_Id);
            this.CreateTable(
                "dbo.RentBookCopies",
                c => new
                    {
                        Rent_Id = c.Int(nullable: false),
                        BookCopy_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Rent_Id, t.BookCopy_Id })
                .ForeignKey("dbo.Rents", t => t.Rent_Id, cascadeDelete: true)
                .ForeignKey("dbo.BookCopies", t => t.BookCopy_Id, cascadeDelete: true)
                .Index(t => t.Rent_Id)
                .Index(t => t.BookCopy_Id);
            this.DropColumn("dbo.Categories", "Book_Id");
            this.DropColumn("dbo.BookCopies", "Rent_Id");
        }

        public override void Down()
        {
            this.AddColumn("dbo.BookCopies", "Rent_Id", c => c.Int());
            this.AddColumn("dbo.Categories", "Book_Id", c => c.Int());
            this.DropForeignKey("dbo.RentBookCopies", "BookCopy_Id", "dbo.BookCopies");
            this.DropForeignKey("dbo.RentBookCopies", "Rent_Id", "dbo.Rents");
            this.DropForeignKey("dbo.CategoryBooks", "Book_Id", "dbo.Books");
            this.DropForeignKey("dbo.CategoryBooks", "Category_Id", "dbo.Categories");
            this.DropIndex("dbo.RentBookCopies", new[] { "BookCopy_Id" });
            this.DropIndex("dbo.RentBookCopies", new[] { "Rent_Id" });
            this.DropIndex("dbo.CategoryBooks", new[] { "Book_Id" });
            this.DropIndex("dbo.CategoryBooks", new[] { "Category_Id" });
            this.DropTable("dbo.RentBookCopies");
            this.DropTable("dbo.CategoryBooks");
            this.CreateIndex("dbo.BookCopies", "Rent_Id");
            this.CreateIndex("dbo.Categories", "Book_Id");
            this.AddForeignKey("dbo.BookCopies", "Rent_Id", "dbo.Rents", "Id");
            this.AddForeignKey("dbo.Categories", "Book_Id", "dbo.Books", "Id");
        }
    }
}
