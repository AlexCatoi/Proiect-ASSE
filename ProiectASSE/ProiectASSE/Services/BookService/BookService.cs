// <copyright file="BookService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.BookService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;
    using ProiectASSE.Services.CategoryService;

    /// <summary>
    /// Provides business logic operations for managing <see cref="Book"/> entities.
    /// </summary>
    public class BookService : IBookService
    {
        /// <summary>
        /// Logger instance for this service.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(BookService));

        private readonly IBookRepository repo;
        private readonly ICategoryService categoryService;
        private readonly IValidator<Book> validator;
        private readonly int maxCategories;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for book data access.</param>
        /// <param name="categoryService">The service used to evaluate category hierarchy.</param>
        /// <param name="validator">The validator used to validate book entities.</param>
        public BookService(
            IBookRepository repo,
            ICategoryService categoryService,
            IValidator<Book> validator)
        {
            this.repo = repo;
            this.categoryService = categoryService;
            this.validator = validator;

            this.maxCategories = int.TryParse(ConfigurationManager.AppSettings["Domenii"], out int val)
                ? val
                : 5;
        }

        /// <inheritdoc/>
        public Book GetBook(int id) => this.repo.GetById(id);

        /// <inheritdoc/>
        public IEnumerable<Book> GetAllBooks() => this.repo.GetAll();

        /// <inheritdoc/>
        public void AddBook(Book book)
        {
            this.ValidateBook(book);

            this.repo.Add(book);
            this.repo.SaveChanges();

            Log.Info($"Book added successfully: '{book.Title}' (ID={book.Id})");
        }

        /// <inheritdoc/>
        public void UpdateBook(Book book)
        {
            this.ValidateBook(book);

            this.repo.Update(book);
            this.repo.SaveChanges();

            Log.Info($"Book updated successfully: '{book.Title}' (ID={book.Id})");
        }

        /// <inheritdoc/>
        public void DeleteBook(int id)
        {
            this.repo.Delete(id);
            this.repo.SaveChanges();

            Log.Warn($"Book deleted: ID={id}");
        }

        /// <inheritdoc/>
        public bool CanBeBorrowed(int bookId)
        {
            var book = this.repo.GetById(bookId);

            if (book == null)
            {
                Log.Warn($"Borrow check failed: BookID={bookId} does not exist.");
                return false;
            }

            int total = book.Copies.Count;
            int readingRoomOnly = book.Copies.Count(c => c.IsReadingRoomOnly);
            int availableForBorrow = book.Copies.Count(c => !c.IsReadingRoomOnly && !c.IsBorrowed);

            if (readingRoomOnly == total)
            {
                Log.Warn($"Borrow denied: All copies of BookID={bookId} are reading-room-only.");
                return false;
            }

            bool ok = availableForBorrow >= Math.Ceiling(total * 0.1);

            if (!ok)
            {
                Log.Warn($"Borrow denied: BookID={bookId} has less than 10% available copies.");
            }
            else
            {
                Log.Info($"BookID={bookId} is available for borrowing.");
            }

            return ok;
        }

        /// <inheritdoc/>
        public IEnumerable<Category> GetAllCategoriesForBook(int bookId)
        {
            var book = this.repo.GetById(bookId);

            if (book == null)
            {
                Log.Warn($"Category retrieval failed: BookID={bookId} does not exist.");
                return Enumerable.Empty<Category>();
            }

            var result = new HashSet<Category>(book.Categories);

            foreach (var cat in book.Categories)
            {
                foreach (var ancestor in this.categoryService.GetAncestors(cat.Id))
                {
                    result.Add(ancestor);
                }
            }

            Log.Info($"Retrieved {result.Count} categories (including ancestors) for BookID={bookId}.");

            return result;
        }

        /// <summary>
        /// Validates a book entity using the configured validator
        /// and performs additional business rule checks.
        /// </summary>
        private void ValidateBook(Book book)
        {
            ValidationResult result = this.validator.Validate(book);

            if (!result.IsValid)
            {
                Log.Warn($"Validation failed for BookID={book.Id}: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }

            this.ValidateCategories(book);
            this.ValidateCopies(book);
        }

        private void ValidateCategories(Book book)
        {
            if (book.Categories == null || !book.Categories.Any())
            {
                Log.Warn($"Book '{book.Title}' has no categories assigned.");
                throw new ValidationException("Book must have at least one category");
            }

            if (book.Categories.Count > this.maxCategories)
            {
                Log.Warn($"Book '{book.Title}' exceeds max categories ({this.maxCategories}).");
                throw new ValidationException($"Book cannot have more than {this.maxCategories} categories");
            }

            foreach (var c1 in book.Categories)
            {
                foreach (var c2 in book.Categories)
                {
                    if (c1.Id == c2.Id)
                    {
                        continue;
                    }

                    if (this.categoryService.IsAncestor(c1.Id, c2.Id) ||
                        this.categoryService.IsAncestor(c2.Id, c1.Id))
                    {
                        Log.Warn($"Invalid category pair for Book '{book.Title}': {c1.Name} ↔ {c2.Name} (ancestor/descendant).");
                        throw new ValidationException(
                            $"Categories {c1.Name} and {c2.Name} cannot both be assigned because they are ancestor/descendant");
                    }
                }
            }
        }

        private void ValidateCopies(Book book)
        {
            if (book.Copies == null || !book.Copies.Any())
            {
                Log.Warn($"Book '{book.Title}' has no copies.");
                throw new ValidationException("Book must have at least one copy");
            }

            int total = book.Copies.Count;
            int readingRoomOnly = book.Copies.Count(c => c.IsReadingRoomOnly);
            int availableForBorrow = book.Copies.Count(c => !c.IsReadingRoomOnly && !c.IsBorrowed);

            if (readingRoomOnly == total)
            {
                Log.Warn($"All copies of Book '{book.Title}' are reading-room-only.");
                throw new ValidationException("All copies are reading-room-only");
            }

            if (availableForBorrow < Math.Ceiling(total * 0.1))
            {
                Log.Warn($"Book '{book.Title}' has less than 10% available copies.");
                throw new ValidationException("Less than 10% of copies are available for borrowing");
            }
        }
    }
}
