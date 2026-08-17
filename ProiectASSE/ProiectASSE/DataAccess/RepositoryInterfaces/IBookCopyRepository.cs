// <copyright file="IBookCopyRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="BookCopy"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface IBookCopyRepository
    {
        /// <summary>
        /// Retrieves a book copy by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the book copy to retrieve.</param>
        /// <returns>
        /// The <see cref="BookCopy"/> with the specified identifier,
        /// or <c>null</c> if no matching book copy is found.
        /// </returns>
        BookCopy GetById(int id);

        /// <summary>
        /// Retrieves all book copies from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="BookCopy"/> entities.</returns>
        IEnumerable<BookCopy> GetAll();

        /// <summary>
        /// Adds a new book copy to the data source.
        /// </summary>
        /// <param name="copy">The book copy entity to add.</param>
        void Add(BookCopy copy);

        /// <summary>
        /// Updates an existing book copy in the data source.
        /// </summary>
        /// <param name="copy">The book copy entity with updated values.</param>
        void Update(BookCopy copy);

        /// <summary>
        /// Deletes a book copy with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the book copy to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();
    }
}
