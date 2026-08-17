// <copyright file="LibraryContext.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Context
{
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Represents the Entity Framework database context for the library system.
    /// This context manages all entity sets and provides access to the underlying database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LibraryContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryContext"/> class
        /// using the connection string defined in the application configuration.
        /// </summary>
        public LibraryContext()
            : base("name=MyConnectionString")
        {
        }

        /// <summary>
        /// Gets or sets the collection of books stored in the library.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Gets or sets the collection of physical book copies available in the library.
        /// </summary>
        public DbSet<BookCopy> BookCopies { get; set; }

        /// <summary>
        /// Gets or sets the collection of rent records representing book borrowings.
        /// </summary>
        public DbSet<Rent> Rents { get; set; }

        /// <summary>
        /// Gets or sets the collection of registered readers.
        /// </summary>
        public DbSet<Reader> Readers { get; set; }

        /// <summary>
        /// Gets or sets the collection of book categories.
        /// Categories may be hierarchical and represent domains and subdomains.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the collection of authors who contributed to books.
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets the collection of book editions.
        /// Each edition may contain specific publication metadata.
        /// </summary>
        public DbSet<Edition> Editions { get; set; }
    }
}
