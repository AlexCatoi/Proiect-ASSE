// <copyright file="IRentHistoryService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using System;
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides access to historical rent information for readers.
    /// Implementations of this interface support querying active rents,
    /// time‑bounded rent history, book‑specific history, daily activity,
    /// and extension statistics.
    /// </summary>
    public interface IRentHistoryService
    {
        /// <summary>
        /// Retrieves all active (non‑returned) rents for the specified reader.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <returns>A collection of active <see cref="Rent"/> entities.</returns>
        IEnumerable<Rent> GetActiveRents(int readerId);

        /// <summary>
        /// Retrieves all rents for the specified reader within a given time interval.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="start">The start date of the interval.</param>
        /// <param name="end">The end date of the interval.</param>
        /// <returns>A collection of <see cref="Rent"/> entities within the specified period.</returns>
        IEnumerable<Rent> GetRentsInPeriod(int readerId, DateTime start, DateTime end);

        /// <summary>
        /// Retrieves all rents for a specific book borrowed by the specified reader.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="bookId">The identifier of the book.</param>
        /// <returns>A collection of <see cref="Rent"/> entities involving the specified book.</returns>
        IEnumerable<Rent> GetRentsForBook(int readerId, int bookId);

        /// <summary>
        /// Retrieves all rents created by the specified reader on a given date.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="date">The date for which rents are requested.</param>
        /// <returns>A collection of <see cref="Rent"/> entities created on the specified date.</returns>
        IEnumerable<Rent> GetRentsForDay(int readerId, DateTime date);

        /// <summary>
        /// Retrieves all rents with extensions performed by the specified reader
        /// within the last three months.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <returns>
        /// A collection of <see cref="Rent"/> entities that include extensions
        /// within the last three months.
        /// </returns>
        IEnumerable<Rent> GetExtensionsInLast3Months(int readerId);
    }
}
