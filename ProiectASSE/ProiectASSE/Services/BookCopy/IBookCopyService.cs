// <copyright file="IBookCopyService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.BookCopyService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business logic operations for managing <see cref="BookCopy"/> entities.
    /// Implementations of this interface handle validation, borrowing rules,
    /// and interactions with the underlying data access layer.
    /// </summary>
    public interface IBookCopyService
    {
        /// <summary>
        /// Retrieves a book copy by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the book copy to retrieve.</param>
        /// <returns>
        /// The <see cref="BookCopy"/> with the specified identifier,
        /// or <c>null</c> if no matching copy is found.
        /// </returns>
        BookCopy GetCopy(int id);

        /// <summary>
        /// Retrieves all book copies from the system.
        /// </summary>
        /// <returns>A collection of all <see cref="BookCopy"/> entities.</returns>
        IEnumerable<BookCopy> GetAllCopies();

        /// <summary>
        /// Adds a new book copy to the system after validation.
        /// </summary>
        /// <param name="copy">The book copy entity to add.</param>
        void AddCopy(BookCopy copy);

        /// <summary>
        /// Updates an existing book copy after validation.
        /// </summary>
        /// <param name="copy">The book copy entity with updated values.</param>
        void UpdateCopy(BookCopy copy);

        /// <summary>
        /// Deletes a book copy with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the book copy to delete.</param>
        void DeleteCopy(int id);

        /// <summary>
        /// Determines whether a specific book copy can be borrowed,
        /// based on its borrowing restrictions and current status.
        /// </summary>
        /// <param name="copyId">The identifier of the book copy to evaluate.</param>
        /// <returns>
        /// <c>true</c> if the copy is borrowable; otherwise, <c>false</c>.
        /// </returns>
        bool CanBeBorrowed(int copyId);
    }
}
