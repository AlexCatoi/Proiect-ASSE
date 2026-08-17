// <copyright file="ICategoryService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.CategoryService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business logic operations for managing <see cref="Category"/> entities.
    /// Implementations of this interface handle validation, hierarchical checks,
    /// and interactions with the underlying data access layer.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to retrieve.</param>
        /// <returns>
        /// The <see cref="Category"/> with the specified identifier,
        /// or <c>null</c> if no matching category is found.
        /// </returns>
        Category GetCategory(int id);

        /// <summary>
        /// Retrieves all categories from the system.
        /// </summary>
        /// <returns>A collection of all <see cref="Category"/> entities.</returns>
        IEnumerable<Category> GetAllCategories();

        /// <summary>
        /// Adds a new category to the system after validation.
        /// </summary>
        /// <param name="category">The category entity to add.</param>
        void AddCategory(Category category);

        /// <summary>
        /// Updates an existing category after validation.
        /// </summary>
        /// <param name="category">The category entity with updated values.</param>
        void UpdateCategory(Category category);

        /// <summary>
        /// Deletes a category with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to delete.</param>
        void DeleteCategory(int id);

        /// <summary>
        /// Determines whether a category is an ancestor of another category.
        /// </summary>
        /// <param name="ancestorId">The identifier of the potential ancestor category.</param>
        /// <param name="descendantId">The identifier of the potential descendant category.</param>
        /// <returns>
        /// <c>true</c> if the first category is an ancestor of the second; otherwise, <c>false</c>.
        /// </returns>
        bool IsAncestor(int ancestorId, int descendantId);

        /// <summary>
        /// Determines whether a category is a descendant of another category.
        /// </summary>
        /// <param name="descendantId">The identifier of the potential descendant category.</param>
        /// <param name="ancestorId">The identifier of the potential ancestor category.</param>
        /// <returns>
        /// <c>true</c> if the first category is a descendant of the second; otherwise, <c>false</c>.
        /// </returns>
        bool IsDescendant(int descendantId, int ancestorId);

        /// <summary>
        /// Retrieves all ancestor categories of a given category.
        /// </summary>
        /// <param name="categoryId">The identifier of the category whose ancestors are requested.</param>
        /// <returns>A collection of ancestor <see cref="Category"/> entities.</returns>
        IEnumerable<Category> GetAncestors(int categoryId);

        /// <summary>
        /// Retrieves all descendant categories of a given category.
        /// </summary>
        /// <param name="categoryId">The identifier of the category whose descendants are requested.</param>
        /// <returns>A collection of descendant <see cref="Category"/> entities.</returns>
        IEnumerable<Category> GetDescendants(int categoryId);

        /// <summary>
        /// Validates a category entity and ensures that assigning its parent
        /// does not introduce cycles in the category hierarchy.
        /// </summary>
        /// <param name="category">The category entity to validate.</param>
        void ValidateCategoryTree(Category category);
    }
}
