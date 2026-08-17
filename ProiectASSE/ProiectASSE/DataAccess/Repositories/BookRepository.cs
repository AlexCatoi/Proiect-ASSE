// <copyright file="BookRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="Book"/> entities.
    /// This repository encapsulates CRUD operations and interacts with the database context.
    /// </summary>
    public class BookRepository : IBookRepository
    {
        /// <summary>
        /// The database context used to access book data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public BookRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves a book by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the book to retrieve.</param>
        /// <returns>
        /// The <see cref="Book"/> with the specified identifier,
        /// or <c>null</c> if no matching book is found.
        /// </returns>
        public Book GetById(int id)
        {
            return this.context.Books.Find(id);
        }

        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>A collection of all <see cref="Book"/> entities.</returns>
        public IEnumerable<Book> GetAll()
        {
            return this.context.Books.ToList();
        }

        /// <summary>
        /// Adds a new book to the database context.
        /// </summary>
        /// <param name="book">The book entity to add.</param>
        public void Add(Book book)
        {
            this.context.Books.Add(book);
        }

        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="book">The book entity with updated values.</param>
        public void Update(Book book)
        {
            var entry = this.context.Entry(book);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.Books.Attach(book);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Deletes a book with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the book to delete.</param>
        public void Delete(int id)
        {
            var book = this.context.Books.Find(id);
            if (book != null)
            {
                this.context.Books.Remove(book);
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
