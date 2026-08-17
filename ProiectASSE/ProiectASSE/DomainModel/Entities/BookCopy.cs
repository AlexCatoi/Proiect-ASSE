// <copyright file="BookCopy.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a physical copy of a book within the library.
    /// Each copy may have different borrowing restrictions and
    /// can be associated with multiple rent transactions over time.
    /// </summary>
    public class BookCopy
    {
        /// <summary>
        /// Gets or sets the unique identifier of the book copy.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the book to which this copy belongs.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the book associated with this copy.
        /// </summary>
        public Book Book { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this copy
        /// can only be used inside the reading room.
        /// </summary>
        public bool IsReadingRoomOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this copy
        /// is currently borrowed by a reader.
        /// </summary>
        public bool IsBorrowed { get; set; }

        /// <summary>
        /// Gets or sets the collection of rent transactions
        /// that include this book copy.
        /// </summary>
        public ICollection<Rent> Rents { get; set; }
    }
}
