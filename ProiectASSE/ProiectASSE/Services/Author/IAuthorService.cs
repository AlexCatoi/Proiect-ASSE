// <copyright file="IAuthorService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.AuthorService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business logic operations for managing <see cref="Author"/> entities.
    /// Implementations of this interface handle validation and interaction
    /// with the underlying data access layer.
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Retrieves an author by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to retrieve.</param>
        /// <returns>
        /// The <see cref="Author"/> with the specified identifier,
        /// or <c>null</c> if no matching author is found.
        /// </returns>
        Author GetAuthor(int id);

        /// <summary>
        /// Retrieves all authors from the system.
        /// </summary>
        /// <returns>A collection of all <see cref="Author"/> entities.</returns>
        IEnumerable<Author> GetAllAuthors();

        /// <summary>
        /// Adds a new author to the system after validation.
        /// </summary>
        /// <param name="author">The author entity to add.</param>
        void AddAuthor(Author author);

        /// <summary>
        /// Updates an existing author after validation.
        /// </summary>
        /// <param name="author">The author entity with updated values.</param>
        void UpdateAuthor(Author author);

        /// <summary>
        /// Deletes an author with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to delete.</param>
        void DeleteAuthor(int id);
    }
}
