// <copyright file="IReaderRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="Reader"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface IReaderRepository
    {
        /// <summary>
        /// Retrieves a reader by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the reader to retrieve.</param>
        /// <returns>
        /// The <see cref="Reader"/> with the specified identifier,
        /// or <c>null</c> if no matching reader is found.
        /// </returns>
        Reader GetById(int id);

        /// <summary>
        /// Retrieves all readers from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="Reader"/> entities.</returns>
        IEnumerable<Reader> GetAll();

        /// <summary>
        /// Adds a new reader to the data source.
        /// </summary>
        /// <param name="reader">The reader entity to add.</param>
        void Add(Reader reader);

        /// <summary>
        /// Updates an existing reader in the data source.
        /// </summary>
        /// <param name="reader">The reader entity with updated values.</param>
        void Update(Reader reader);

        /// <summary>
        /// Deletes a reader with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the reader to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Determines whether an email address already exists for another reader.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <param name="excludeId">
        /// An optional reader identifier to exclude from the check,
        /// typically used when updating an existing reader.
        /// </param>
        /// <returns>
        /// <c>true</c> if the email exists for another reader; otherwise, <c>false</c>.
        /// </returns>
        bool EmailExists(string email, int? excludeId = null);

        /// <summary>
        /// Determines whether a phone number already exists for another reader.
        /// </summary>
        /// <param name="phone">The phone number to check.</param>
        /// <param name="excludeId">
        /// An optional reader identifier to exclude from the check,
        /// typically used when updating an existing reader.
        /// </param>
        /// <returns>
        /// <c>true</c> if the phone number exists for another reader; otherwise, <c>false</c>.
        /// </returns>
        bool PhoneExists(string phone, int? excludeId = null);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();
    }
}
