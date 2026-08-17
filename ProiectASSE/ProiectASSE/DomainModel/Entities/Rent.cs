// <copyright file="Rent.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rent transaction within the library system.
    /// A rent links a reader to one or more borrowed book copies,
    /// tracks due dates, extensions, and return information.
    /// </summary>
    public class Rent
    {
        /// <summary>
        /// Gets or sets the unique identifier of the rent record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the reader who initiated the rent.
        /// </summary>
        public int ReaderId { get; set; }

        /// <summary>
        /// Gets or sets the reader associated with the rent.
        /// </summary>
        public Reader Reader { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the employee who processed the rent,
        /// if applicable.
        /// </summary>
        public int? ProcessedByEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the employee who processed the rent.
        /// </summary>
        public Employee ProcessedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the date when the rent was initiated.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the rented items are due to be returned.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the rented items were actually returned.
        /// A <c>null</c> value indicates that the items have not yet been returned.
        /// </summary>
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// Gets or sets the total number of extension days granted for the rent.
        /// </summary>
        public int ExtensionDaysTotal { get; set; }

        /// <summary>
        /// Gets or sets the number of times the rent has been extended.
        /// </summary>
        public int NumberOfExtensions { get; set; }

        /// <summary>
        /// Gets or sets the current status of the rent.
        /// </summary>
        public RentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the collection of book copies included in the rent.
        /// </summary>
        public ICollection<BookCopy> BookCopies { get; set; }
    }

    /// <summary>
    /// Represents the possible statuses of a rent transaction.
    /// </summary>
    public enum RentStatus
    {
        /// <summary>
        /// Indicates that the rent is currently active and the items have not been returned.
        /// </summary>
        ACTIVE,

        /// <summary>
        /// Indicates that all rented items have been returned.
        /// </summary>
        RETURNED,

        /// <summary>
        /// Indicates that the rent is overdue and the due date has passed.
        /// </summary>
        OVERDUE,
    }
}
