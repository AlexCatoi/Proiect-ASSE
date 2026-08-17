// <copyright file="Book.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a book in the library system.
    /// A book may belong to multiple categories, have multiple authors,
    /// and be available in multiple editions and physical copies.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier of the book.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a textual description of the book.
        /// This may include a summary, abstract, or additional notes.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of categories to which the book belongs.
        /// A book must belong to at least one category, and categories may be hierarchical.
        /// </summary>
        public ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the collection of authors who contributed to the book.
        /// A book may have one or more authors.
        /// </summary>
        public ICollection<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets the collection of editions in which the book was published.
        /// Each edition may contain specific metadata such as page count or publication year.
        /// </summary>
        public ICollection<Edition> Editions { get; set; }

        /// <summary>
        /// Gets or sets the collection of physical copies of the book available in the library.
        /// Copies may be borrowable or restricted to the reading room.
        /// </summary>
        public ICollection<BookCopy> Copies { get; set; }
    }
}
