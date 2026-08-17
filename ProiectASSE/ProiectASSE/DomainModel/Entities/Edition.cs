// <copyright file="Edition.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    /// <summary>
    /// Represents a specific edition of a book, including publisher details,
    /// publication year, physical characteristics, and the associated book.
    /// </summary>
    public class Edition
    {
        /// <summary>
        /// Gets or sets the unique identifier of the edition.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the publisher of the edition.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the year when the edition was published.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the edition.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the type of book (e.g., hardcover, paperback).
        /// </summary>
        public string BookType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the book to which this edition belongs.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the book associated with this edition.
        /// </summary>
        public Book Book { get; set; }
    }
}
