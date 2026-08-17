// <copyright file="202601031133240_InitialMigration.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            this.CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Int(),
                        Book_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.ParentId)
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .Index(t => t.ParentId)
                .Index(t => t.Book_Id);

            this.CreateTable(
                "dbo.BookCopies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        IsReadingRoomOnly = c.Boolean(nullable: false),
                        IsBorrowed = c.Boolean(nullable: false),
                        Rent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Rents", t => t.Rent_Id)
                .Index(t => t.BookId)
                .Index(t => t.Rent_Id);

            this.CreateTable(
                "dbo.Editions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Publisher = c.String(),
                        Year = c.Int(nullable: false),
                        Pages = c.Int(nullable: false),
                        BookType = c.String(),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);

            this.CreateTable(
                "dbo.Readers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 50),
                        Email = c.String(),
                        Phone = c.String(),
                        EnrollDate = c.DateTime(nullable: false),
                        EmployDate = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.Rents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReaderId = c.Int(nullable: false),
                        ProcessedByEmployeeId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(),
                        ExtensionDaysTotal = c.Int(nullable: false),
                        NumberOfExtensions = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Readers", t => t.ProcessedByEmployeeId)
                .ForeignKey("dbo.Readers", t => t.ReaderId, cascadeDelete: true)
                .Index(t => t.ReaderId)
                .Index(t => t.ProcessedByEmployeeId);

            this.CreateTable(
                "dbo.BookAuthors",
                c => new
                    {
                        Book_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_Id, t.Author_Id })
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Book_Id)
                .Index(t => t.Author_Id);
        }

        public override void Down()
        {
            this.DropForeignKey("dbo.Rents", "ReaderId", "dbo.Readers");
            this.DropForeignKey("dbo.Rents", "ProcessedByEmployeeId", "dbo.Readers");
            this.DropForeignKey("dbo.BookCopies", "Rent_Id", "dbo.Rents");
            this.DropForeignKey("dbo.Editions", "BookId", "dbo.Books");
            this.DropForeignKey("dbo.BookCopies", "BookId", "dbo.Books");
            this.DropForeignKey("dbo.Categories", "Book_Id", "dbo.Books");
            this.DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            this.DropForeignKey("dbo.BookAuthors", "Author_Id", "dbo.Authors");
            this.DropForeignKey("dbo.BookAuthors", "Book_Id", "dbo.Books");
            this.DropIndex("dbo.BookAuthors", new[] { "Author_Id" });
            this.DropIndex("dbo.BookAuthors", new[] { "Book_Id" });
            this.DropIndex("dbo.Rents", new[] { "ProcessedByEmployeeId" });
            this.DropIndex("dbo.Rents", new[] { "ReaderId" });
            this.DropIndex("dbo.Editions", new[] { "BookId" });
            this.DropIndex("dbo.BookCopies", new[] { "Rent_Id" });
            this.DropIndex("dbo.BookCopies", new[] { "BookId" });
            this.DropIndex("dbo.Categories", new[] { "Book_Id" });
            this.DropIndex("dbo.Categories", new[] { "ParentId" });
            this.DropTable("dbo.BookAuthors");
            this.DropTable("dbo.Rents");
            this.DropTable("dbo.Readers");
            this.DropTable("dbo.Editions");
            this.DropTable("dbo.BookCopies");
            this.DropTable("dbo.Categories");
            this.DropTable("dbo.Books");
            this.DropTable("dbo.Authors");
        }
    }
}
