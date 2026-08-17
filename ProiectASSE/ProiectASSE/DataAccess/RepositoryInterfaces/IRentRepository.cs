// <copyright file="IRentRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="Rent"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface IRentRepository
    {
        /// <summary>
        /// Retrieves a rent record by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the rent record to retrieve.</param>
        /// <returns>
        /// The <see cref="Rent"/> with the specified identifier,
        /// or <c>null</c> if no matching rent record is found.
        /// </returns>
        Rent GetById(int id);

        /// <summary>
        /// Retrieves all rent records from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="Rent"/> entities.</returns>
        IEnumerable<Rent> GetAll();

        /// <summary>
        /// Adds a new rent record to the data source.
        /// </summary>
        /// <param name="rent">The rent entity to add.</param>
        void Add(Rent rent);

        /// <summary>
        /// Updates an existing rent record in the data source.
        /// </summary>
        /// <param name="rent">The rent entity with updated values.</param>
        void Update(Rent rent);

        /// <summary>
        /// Deletes a rent record with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the rent record to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Retrieves all active (not yet returned) rents for a specific reader.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <returns>A collection of active <see cref="Rent"/> records.</returns>
        IEnumerable<Rent> GetActiveRentsForReader(int readerId);

        /// <summary>
        /// Retrieves all rents for a reader within a specified time period.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="start">The start date of the period.</param>
        /// <param name="end">The end date of the period.</param>
        /// <returns>A collection of <see cref="Rent"/> records within the specified period.</returns>
        IEnumerable<Rent> GetRentsForReaderInPeriod(int readerId, DateTime start, DateTime end);

        /// <summary>
        /// Retrieves all rents for a reader that include a specific book.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="bookId">The identifier of the book.</param>
        /// <returns>A collection of <see cref="Rent"/> records matching the criteria.</returns>
        IEnumerable<Rent> GetRentsForReaderAndBook(int readerId, int bookId);

        /// <summary>
        /// Retrieves all rents for a reader that were active on a specific date.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="date">The date to check.</param>
        /// <returns>A collection of <see cref="Rent"/> records active on the specified date.</returns>
        IEnumerable<Rent> GetRentsForReaderOnDate(int readerId, DateTime date);

        /// <summary>
        /// Retrieves all rent extensions made by a reader in the last three months.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <returns>
        /// A collection of <see cref="Rent"/> records that include extensions
        /// within the last three months.
        /// </returns>
        IEnumerable<Rent> GetExtensionsForReaderInLast3Months(int readerId);
    }
}
