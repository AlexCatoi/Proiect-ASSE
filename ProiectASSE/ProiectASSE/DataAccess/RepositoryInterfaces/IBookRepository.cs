// <copyright file="IBookRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="Book"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Retrieves a book by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the book to retrieve.</param>
        /// <returns>
        /// The <see cref="Book"/> with the specified identifier,
        /// or <c>null</c> if no matching book is found.
        /// </returns>
        Book GetById(int id);

        /// <summary>
        /// Retrieves all books from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="Book"/> entities.</returns>
        IEnumerable<Book> GetAll();

        /// <summary>
        /// Adds a new book to the data source.
        /// </summary>
        /// <param name="book">The book entity to add.</param>
        void Add(Book book);

        /// <summary>
        /// Updates an existing book in the data source.
        /// </summary>
        /// <param name="book">The book entity with updated values.</param>
        void Update(Book book);

        /// <summary>
        /// Deletes a book with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the book to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();
    }
}
