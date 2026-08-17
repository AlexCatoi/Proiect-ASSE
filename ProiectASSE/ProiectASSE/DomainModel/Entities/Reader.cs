// <copyright file="Reader.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    using System;

    /// <summary>
    /// Represents a library reader who can borrow books and maintain
    /// contact information within the library system.
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// Gets or sets the unique identifier of the reader.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the reader.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the reader.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the email address of the reader.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the reader.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the date when the reader enrolled in the library.
        /// </summary>
        public DateTime EnrollDate { get; set; }
    }
}
