// <copyright file="AuthorRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="Author"/> entities.
    /// This repository encapsulates CRUD operations and interacts with the database context.
    /// </summary>
    public class AuthorRepository : IAuthorRepository
    {
        /// <summary>
        /// The database context used to access author data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public AuthorRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves an author by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to retrieve.</param>
        /// <returns>
        /// The <see cref="Author"/> with the specified identifier,
        /// or <c>null</c> if no matching author is found.
        /// </returns>
        public Author GetById(int id)
        {
            return this.context.Authors.Find(id);
        }

        /// <summary>
        /// Retrieves all authors from the database.
        /// </summary>
        /// <returns>A collection of all <see cref="Author"/> entities.</returns>
        public IEnumerable<Author> GetAll()
        {
            return this.context.Authors.ToList();
        }

        /// <summary>
        /// Adds a new author to the database context.
        /// </summary>
        /// <param name="author">The author entity to add.</param>
        public void Add(Author author)
        {
            this.context.Authors.Add(author);
        }

        /// <summary>
        /// Updates an existing author in the database.
        /// </summary>
        /// <param name="author">The author entity with updated values.</param>
        public void Update(Author author)
        {
            var entry = this.context.Entry(author);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.Authors.Attach(author);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Deletes an author with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to delete.</param>
        public void Delete(int id)
        {
            var author = this.context.Authors.Find(id);
            if (author != null)
            {
                this.context.Authors.Remove(author);
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
