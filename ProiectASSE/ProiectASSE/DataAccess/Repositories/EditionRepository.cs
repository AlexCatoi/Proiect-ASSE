// <copyright file="EditionRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="Edition"/> entities.
    /// This repository encapsulates CRUD operations and interacts with the database context.
    /// </summary>
    public class EditionRepository : IEditionRepository
    {
        /// <summary>
        /// The database context used to access edition data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public EditionRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves an edition by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the edition to retrieve.</param>
        /// <returns>
        /// The <see cref="Edition"/> with the specified identifier,
        /// or <c>null</c> if no matching edition is found.
        /// </returns>
        public Edition GetById(int id)
        {
            return this.context.Editions.Find(id);
        }

        /// <summary>
        /// Retrieves all editions from the database.
        /// </summary>
        /// <returns>A collection of all <see cref="Edition"/> entities.</returns>
        public IEnumerable<Edition> GetAll()
        {
            return this.context.Editions.ToList();
        }

        /// <summary>
        /// Adds a new edition to the database context.
        /// </summary>
        /// <param name="edition">The edition entity to add.</param>
        public void Add(Edition edition)
        {
            this.context.Editions.Add(edition);
        }

        /// <summary>
        /// Updates an existing edition in the database.
        /// </summary>
        /// <param name="edition">The edition entity with updated values.</param>
        public void Update(Edition edition)
        {
            var entry = this.context.Entry(edition);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.Editions.Attach(edition);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Deletes an edition with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the edition to delete.</param>
        public void Delete(int id)
        {
            var edition = this.context.Editions.Find(id);
            if (edition != null)
            {
                this.context.Editions.Remove(edition);
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
