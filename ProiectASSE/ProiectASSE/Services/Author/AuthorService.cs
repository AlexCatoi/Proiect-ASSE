// <copyright file="AuthorService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.AuthorService
{
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Results;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides business logic operations for managing <see cref="Author"/> entities.
    /// This service handles validation and delegates data persistence to the repository layer.
    /// </summary>
    public class AuthorService : IAuthorService
    {
        /// <summary>
        /// Logger instance for this service.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthorService));

        /// <summary>
        /// The repository used to access author data.
        /// </summary>
        private readonly IAuthorRepository repo;

        /// <summary>
        /// The validator used to validate <see cref="Author"/> entities.
        /// </summary>
        private readonly IValidator<Author> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for author data access.</param>
        /// <param name="validator">The validator used to validate author entities.</param>
        public AuthorService(IAuthorRepository repo, IValidator<Author> validator)
        {
            this.repo = repo;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public Author GetAuthor(int id)
        {
            return this.repo.GetById(id);
        }

        /// <inheritdoc/>
        public IEnumerable<Author> GetAllAuthors() => this.repo.GetAll();

        /// <inheritdoc/>
        public void AddAuthor(Author author)
        {
            this.Validate(author);

            this.repo.Add(author);
            this.repo.SaveChanges();

            Log.Info($"Author added successfully: {author.FirstName} {author.LastName} (ID={author.Id})");
        }

        /// <inheritdoc/>
        public void UpdateAuthor(Author author)
        {
            this.Validate(author);

            this.repo.Update(author);
            this.repo.SaveChanges();

            Log.Info($"Author updated successfully: {author.FirstName} {author.LastName} (ID={author.Id})");
        }

        /// <inheritdoc/>
        public void DeleteAuthor(int id)
        {
            this.repo.Delete(id);
            this.repo.SaveChanges();

            Log.Warn($"Author deleted: ID={id}");
        }

        /// <summary>
        /// Validates an author entity using the configured validator.
        /// Throws a <see cref="ValidationException"/> if validation fails.
        /// </summary>
        private void Validate(Author author)
        {
            ValidationResult result = this.validator.Validate(author);

            if (!result.IsValid)
            {
                Log.Warn($"Validation failed for author ID={author.Id}: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }
        }
    }
}
