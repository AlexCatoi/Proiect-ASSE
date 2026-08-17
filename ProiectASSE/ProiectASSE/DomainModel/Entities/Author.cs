// <copyright file="Author.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an author who has written one or more books.
    /// Contains basic identity information and a navigation property
    /// to the books associated with the author.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets the unique identifier of the author.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the author's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the author's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the collection of books written by the author.
        /// </summary>
        public ICollection<Book> Books { get; set; }
    }
}
