// <copyright file="ICategoryRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="Category"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to retrieve.</param>
        /// <returns>
        /// The <see cref="Category"/> with the specified identifier,
        /// or <c>null</c> if no matching category is found.
        /// </returns>
        Category GetById(int id);

        /// <summary>
        /// Retrieves all categories from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="Category"/> entities.</returns>
        IEnumerable<Category> GetAll();

        /// <summary>
        /// Adds a new category to the data source.
        /// </summary>
        /// <param name="category">The category entity to add.</param>
        void Add(Category category);

        /// <summary>
        /// Updates an existing category in the data source.
        /// </summary>
        /// <param name="category">The category entity with updated values.</param>
        void Update(Category category);

        /// <summary>
        /// Deletes a category with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();
    }
}
