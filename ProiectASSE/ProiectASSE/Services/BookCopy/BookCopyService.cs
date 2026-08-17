// <copyright file="BookCopyService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.BookCopyService
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides business logic operations for managing <see cref="BookCopy"/> entities.
    /// This service handles validation, borrowing rules, and delegates persistence
    /// operations to the repository layer.
    /// </summary>
    public class BookCopyService : IBookCopyService
    {
        /// <summary>
        /// Logger instance for this service.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(BookCopyService));

        /// <summary>
        /// The repository used to access book copy data.
        /// </summary>
        private readonly IBookCopyRepository repo;

        /// <summary>
        /// The validator used to validate <see cref="BookCopy"/> entities.
        /// </summary>
        private readonly IValidator<BookCopy> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookCopyService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for book copy data access.</param>
        /// <param name="validator">The validator used to validate book copy entities.</param>
        public BookCopyService(IBookCopyRepository repo, IValidator<BookCopy> validator)
        {
            this.repo = repo;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public BookCopy GetCopy(int id)
        {
            return this.repo.GetById(id);
        }

        /// <inheritdoc/>
        public IEnumerable<BookCopy> GetAllCopies() => this.repo.GetAll();

        /// <inheritdoc/>
        public void AddCopy(BookCopy copy)
        {
            this.Validate(copy);

            this.repo.Add(copy);
            this.repo.SaveChanges();

            Log.Info($"Book copy added successfully: CopyID={copy.Id}, BookID={copy.BookId}");
        }

        /// <inheritdoc/>
        public void UpdateCopy(BookCopy copy)
        {
            this.Validate(copy);

            this.repo.Update(copy);
            this.repo.SaveChanges();

            Log.Info($"Book copy updated successfully: CopyID={copy.Id}, BookID={copy.BookId}");
        }

        /// <inheritdoc/>
        public void DeleteCopy(int id)
        {
            this.repo.Delete(id);
            this.repo.SaveChanges();

            Log.Warn($"Book copy deleted: CopyID={id}");
        }

        /// <inheritdoc/>
        public bool CanBeBorrowed(int copyId)
        {
            var copy = this.repo.GetById(copyId);

            if (copy == null)
            {
                Log.Warn($"Borrow check failed: CopyID={copyId} does not exist.");
                return false;
            }

            if (copy.IsReadingRoomOnly)
            {
                Log.Warn($"Borrow denied: CopyID={copyId} is reading-room only.");
                return false;
            }

            if (copy.IsBorrowed)
            {
                Log.Warn($"Borrow denied: CopyID={copyId} is already borrowed.");
                return false;
            }

            bool hasActiveRent = copy.Rents?.Any(r => r.Status == RentStatus.ACTIVE) ?? false;
            if (hasActiveRent)
            {
                Log.Warn($"Borrow denied: CopyID={copyId} has an active rent.");
                return false;
            }

            Log.Info($"CopyID={copyId} is available for borrowing.");
            return true;
        }

        /// <summary>
        /// Validates a book copy entity using the configured validator.
        /// </summary>
        private void Validate(BookCopy copy)
        {
            ValidationResult result = this.validator.Validate(copy);

            if (!result.IsValid)
            {
                Log.Warn($"Validation failed for BookCopy ID={copy.Id}: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }
        }
    }
}
