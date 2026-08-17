// <copyright file="BookCopyRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="BookCopy"/> entities.
    /// This repository encapsulates CRUD operations and interacts with the database context.
    /// </summary>
    public class BookCopyRepository : IBookCopyRepository
    {
        /// <summary>
        /// The database context used to access book copy data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookCopyRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public BookCopyRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves a book copy by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the book copy to retrieve.</param>
        /// <returns>
        /// The <see cref="BookCopy"/> with the specified identifier,
        /// or <c>null</c> if no matching copy is found.
        /// </returns>
        public BookCopy GetById(int id)
        {
            return this.context.BookCopies.Find(id);
        }

        /// <summary>
        /// Retrieves all book copies from the database.
        /// </summary>
        /// <returns>A collection of all <see cref="BookCopy"/> entities.</returns>
        public IEnumerable<BookCopy> GetAll()
        {
            return this.context.BookCopies.ToList();
        }

        /// <summary>
        /// Adds a new book copy to the database context.
        /// </summary>
        /// <param name="copy">The book copy entity to add.</param>
        public void Add(BookCopy copy)
        {
            this.context.BookCopies.Add(copy);
        }

        /// <summary>
        /// Updates an existing book copy in the database.
        /// </summary>
        /// <param name="copy">The book copy entity with updated values.</param>
        public void Update(BookCopy copy)
        {
            var entry = this.context.Entry(copy);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.BookCopies.Attach(copy);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Deletes a book copy with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the book copy to delete.</param>
        public void Delete(int id)
        {
            var copy = this.context.BookCopies.Find(id);
            if (copy != null)
            {
                this.context.BookCopies.Remove(copy);
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
