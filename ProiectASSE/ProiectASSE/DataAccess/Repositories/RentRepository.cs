// <copyright file="RentRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="Rent"/> entities.
    /// This repository encapsulates CRUD operations and interacts with the database context.
    /// </summary>
    public class RentRepository : IRentRepository
    {
        /// <summary>
        /// The database context used to access rent data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public RentRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves a rent record by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the rent record to retrieve.</param>
        /// <returns>
        /// The <see cref="Rent"/> with the specified identifier,
        /// or <c>null</c> if no matching rent record is found.
        /// </returns>
        public Rent GetById(int id)
        {
            return this.context.Rents.Find(id);
        }

        /// <summary>
        /// Retrieves all rent records from the database.
        /// </summary>
        /// <returns>A collection of all <see cref="Rent"/> entities.</returns>
        public IEnumerable<Rent> GetAll()
        {
            return this.context.Rents.ToList();
        }

        /// <summary>
        /// Adds a new rent record to the database context.
        /// </summary>
        /// <param name="rent">The rent entity to add.</param>
        public void Add(Rent rent)
        {
            this.context.Rents.Add(rent);
        }

        /// <summary>
        /// Updates an existing rent record in the database.
        /// </summary>
        /// <param name="rent">The rent entity with updated values.</param>
        public void Update(Rent rent)
        {
            var entry = this.context.Entry(rent);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.Rents.Attach(rent);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Deletes a rent record with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the rent record to delete.</param>
        public void Delete(int id)
        {
            var rent = this.context.Rents.Find(id);
            if (rent != null)
            {
                this.context.Rents.Remove(rent);
            }
        }

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        /// <summary>
        /// Retrieves all active (not yet returned) rents for a specific reader.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <returns>A collection of active <see cref="Rent"/> records.</returns>
        public IEnumerable<Rent> GetActiveRentsForReader(int readerId)
        {
            return this.context.Rents
                .Where(r => r.ReaderId == readerId && r.ReturnDate == null)
                .ToList();
        }

        /// <summary>
        /// Retrieves all rents for a reader within a specified time period.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="start">The start date of the period.</param>
        /// <param name="end">The end date of the period.</param>
        /// <returns>A collection of <see cref="Rent"/> records within the specified period.</returns>
        public IEnumerable<Rent> GetRentsForReaderInPeriod(int readerId, DateTime start, DateTime end)
        {
            return this.context.Rents
                .Where(r => r.ReaderId == readerId &&
                            r.StartDate >= start &&
                            r.StartDate <= end)
                .ToList();
        }

        /// <summary>
        /// Retrieves all rents for a reader that include a specific book.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="bookId">The identifier of the book.</param>
        /// <returns>A collection of <see cref="Rent"/> records matching the criteria.</returns>
        public IEnumerable<Rent> GetRentsForReaderAndBook(int readerId, int bookId)
        {
            return this.context.Rents
                .Where(r => r.ReaderId == readerId &&
                            r.BookCopies.Any(bc => bc.BookId == bookId))
                .ToList();
        }

        /// <summary>
        /// Retrieves all rents for a reader that were active on a specific date.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <param name="date">The date to check.</param>
        /// <returns>A collection of <see cref="Rent"/> records active on the specified date.</returns>
        public IEnumerable<Rent> GetRentsForReaderOnDate(int readerId, DateTime date)
        {
            return this.context.Rents
                .Where(r => r.ReaderId == readerId &&
                            r.StartDate <= date &&
                            (r.ReturnDate == null || r.ReturnDate >= date))
                .ToList();
        }

        /// <summary>
        /// Retrieves all rent extensions made by a reader in the last three months.
        /// </summary>
        /// <param name="readerId">The identifier of the reader.</param>
        /// <returns>A collection of <see cref="Rent"/> records with extensions in the last three months.</returns>
        public IEnumerable<Rent> GetExtensionsForReaderInLast3Months(int readerId)
        {
            DateTime cutoff = DateTime.Now.AddMonths(-3);

            return this.context.Rents
                .Where(r => r.ReaderId == readerId &&
                            r.NumberOfExtensions > 0 &&
                            r.DueDate >= cutoff)
                .ToList();
        }
    }
}
