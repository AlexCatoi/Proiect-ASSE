// <copyright file="IEditionRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="Edition"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface IEditionRepository
    {
        /// <summary>
        /// Retrieves an edition by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the edition to retrieve.</param>
        /// <returns>
        /// The <see cref="Edition"/> with the specified identifier,
        /// or <c>null</c> if no matching edition is found.
        /// </returns>
        Edition GetById(int id);

        /// <summary>
        /// Retrieves all editions from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="Edition"/> entities.</returns>
        IEnumerable<Edition> GetAll();

        /// <summary>
        /// Adds a new edition to the data source.
        /// </summary>
        /// <param name="edition">The edition entity to add.</param>
        void Add(Edition edition);

        /// <summary>
        /// Updates an existing edition in the data source.
        /// </summary>
        /// <param name="edition">The edition entity with updated values.</param>
        void Update(Edition edition);

        /// <summary>
        /// Deletes an edition with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the edition to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();
    }
}
