// <copyright file="IAuthorRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="Author"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface IAuthorRepository
    {
        /// <summary>
        /// Retrieves an author by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to retrieve.</param>
        /// <returns>
        /// The <see cref="Author"/> with the specified identifier,
        /// or <c>null</c> if no matching author is found.
        /// </returns>
        Author GetById(int id);

        /// <summary>
        /// Retrieves all authors from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="Author"/> entities.</returns>
        IEnumerable<Author> GetAll();

        /// <summary>
        /// Adds a new author to the data source.
        /// </summary>
        /// <param name="author">The author entity to add.</param>
        void Add(Author author);

        /// <summary>
        /// Updates an existing author in the data source.
        /// </summary>
        /// <param name="author">The author entity with updated values.</param>
        void Update(Author author);

        /// <summary>
        /// Deletes an author with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();
    }
}
