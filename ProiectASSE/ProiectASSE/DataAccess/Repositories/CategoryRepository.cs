// <copyright file="CategoryRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="Category"/> entities.
    /// This repository encapsulates CRUD operations and interacts with the database context.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// The database context used to access category data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public CategoryRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to retrieve.</param>
        /// <returns>
        /// The <see cref="Category"/> with the specified identifier,
        /// or <c>null</c> if no matching category is found.
        /// </returns>
        public Category GetById(int id)
        {
            return this.context.Categories.Find(id);
        }

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>A collection of all <see cref="Category"/> entities.</returns>
        public IEnumerable<Category> GetAll()
        {
            return this.context.Categories.ToList();
        }

        /// <summary>
        /// Adds a new category to the database context.
        /// </summary>
        /// <param name="category">The category entity to add.</param>
        public void Add(Category category)
        {
            this.context.Categories.Add(category);
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="category">The category entity with updated values.</param>
        public void Update(Category category)
        {
            var entry = this.context.Entry(category);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.Categories.Attach(category);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Deletes a category with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to delete.</param>
        public void Delete(int id)
        {
            var category = this.context.Categories.Find(id);
            if (category != null)
            {
                this.context.Categories.Remove(category);
            }
        }

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}
