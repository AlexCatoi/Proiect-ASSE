// <copyright file="IEditionService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.EditionService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business logic operations for managing <see cref="Edition"/> entities.
    /// Implementations of this interface handle validation and interactions
    /// with the underlying data access layer.
    /// </summary>
    public interface IEditionService
    {
        /// <summary>
        /// Retrieves an edition by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the edition to retrieve.</param>
        /// <returns>
        /// The <see cref="Edition"/> with the specified identifier,
        /// or <c>null</c> if no matching edition is found.
        /// </returns>
        Edition GetEdition(int id);

        /// <summary>
        /// Retrieves all editions from the system.
        /// </summary>
        /// <returns>A collection of all <see cref="Edition"/> entities.</returns>
        IEnumerable<Edition> GetAllEditions();

        /// <summary>
        /// Adds a new edition to the system after validation.
        /// </summary>
        /// <param name="edition">The edition entity to add.</param>
        void AddEdition(Edition edition);

        /// <summary>
        /// Updates an existing edition after validation.
        /// </summary>
        /// <param name="edition">The edition entity with updated values.</param>
        void UpdateEdition(Edition edition);

        /// <summary>
        /// Deletes an edition with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the edition to delete.</param>
        void DeleteEdition(int id);
    }
}
