// <copyright file="RentHistoryService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using System;
    using System.Collections.Generic;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides access to historical rent information for readers.
    /// This service delegates all data retrieval operations to the underlying
    /// <see cref="IRentRepository"/> implementation.
    /// </summary>
    public class RentHistoryService : IRentHistoryService
    {
        /// <summary>
        /// The repository used to access rent history data.
        /// </summary>
        private readonly IRentRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentHistoryService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for rent history data access.</param>
        public RentHistoryService(IRentRepository repo)
        {
            this.repo = repo;
        }

        /// <inheritdoc/>
        public IEnumerable<Rent> GetActiveRents(int readerId)
        {
            return this.repo.GetActiveRentsForReader(readerId);
        }

        /// <inheritdoc/>
        public IEnumerable<Rent> GetRentsInPeriod(int readerId, DateTime start, DateTime end)
        {
            return this.repo.GetRentsForReaderInPeriod(readerId, start, end);
        }

        /// <inheritdoc/>
        public IEnumerable<Rent> GetRentsForBook(int readerId, int bookId)
        {
            return this.repo.GetRentsForReaderAndBook(readerId, bookId);
        }

        /// <inheritdoc/>
        public IEnumerable<Rent> GetRentsForDay(int readerId, DateTime date)
        {
            return this.repo.GetRentsForReaderOnDate(readerId, date);
        }

        /// <inheritdoc/>
        public IEnumerable<Rent> GetExtensionsInLast3Months(int readerId)
        {
            return this.repo.GetExtensionsForReaderInLast3Months(readerId);
        }
    }
}
