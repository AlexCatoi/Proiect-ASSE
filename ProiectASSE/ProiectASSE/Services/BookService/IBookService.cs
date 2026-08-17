// <copyright file="IBookService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.BookService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business logic operations for managing <see cref="Book"/> entities.
    /// Implementations of this interface handle validation, borrowing rules,
    /// and interactions with the underlying data access layer.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Retrieves a book by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the book to retrieve.</param>
        /// <returns>
        /// The <see cref="Book"/> with the specified identifier,
        /// or <c>null</c> if no matching book is found.
        /// </returns>
        Book GetBook(int id);

        /// <summary>
        /// Retrieves all books from the system.
        /// </summary>
        /// <returns>A collection of all <see cref="Book"/> entities.</returns>
        IEnumerable<Book> GetAllBooks();

        /// <summary>
        /// Adds a new book to the system after validation.
        /// </summary>
        /// <param name="book">The book entity to add.</param>
        void AddBook(Book book);

        /// <summary>
        /// Updates an existing book after validation.
        /// </summary>
        /// <param name="book">The book entity with updated values.</param>
        void UpdateBook(Book book);

        /// <summary>
        /// Deletes a book with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the book to delete.</param>
        void DeleteBook(int id);

        /// <summary>
        /// Determines whether a book can be borrowed based on its copies
        /// and borrowing restrictions.
        /// </summary>
        /// <param name="bookId">The identifier of the book to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the book has at least one borrowable copy; otherwise, <c>false</c>.
        /// </returns>
        bool CanBeBorrowed(int bookId);

        /// <summary>
        /// Retrieves all categories associated with a given book.
        /// </summary>
        /// <param name="bookId">The identifier of the book whose categories are requested.</param>
        /// <returns>A collection of <see cref="Category"/> entities.</returns>
        IEnumerable<Category> GetAllCategoriesForBook(int bookId);
    }
}
