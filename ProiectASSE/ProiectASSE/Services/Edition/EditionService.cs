// <copyright file="EditionService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.EditionService
{
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Results;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides business logic operations for managing <see cref="Edition"/> entities.
    /// This service handles validation and delegates persistence operations
    /// to the underlying repository layer.
    /// </summary>
    public class EditionService : IEditionService
    {
        /// <summary>
        /// Logger instance for this service.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(EditionService));

        /// <summary>
        /// The repository used to access edition data.
        /// </summary>
        private readonly IEditionRepository repo;

        /// <summary>
        /// The validator used to validate <see cref="Edition"/> entities.
        /// </summary>
        private readonly IValidator<Edition> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditionService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for edition data access.</param>
        /// <param name="validator">The validator used to validate edition entities.</param>
        public EditionService(IEditionRepository repo, IValidator<Edition> validator)
        {
            this.repo = repo;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public Edition GetEdition(int id) => this.repo.GetById(id);

        /// <inheritdoc/>
        public IEnumerable<Edition> GetAllEditions() => this.repo.GetAll();

        /// <inheritdoc/>
        public void AddEdition(Edition edition)
        {
            this.Validate(edition);

            this.repo.Add(edition);
            this.repo.SaveChanges();

            Log.Info($"Edition added successfully: EditionID={edition.Id}, BookID={edition.BookId}");
        }

        /// <inheritdoc/>
        public void UpdateEdition(Edition edition)
        {
            this.Validate(edition);

            this.repo.Update(edition);
            this.repo.SaveChanges();

            Log.Info($"Edition updated successfully: EditionID={edition.Id}, BookID={edition.BookId}");
        }

        /// <inheritdoc/>
        public void DeleteEdition(int id)
        {
            this.repo.Delete(id);
            this.repo.SaveChanges();

            Log.Warn($"Edition deleted: EditionID={id}");
        }

        /// <summary>
        /// Validates an edition entity using the configured validator.
        /// </summary>
        /// <param name="edition">The edition entity to validate.</param>
        /// <exception cref="ValidationException">
        /// Thrown when validation fails.
        /// </exception>
        private void Validate(Edition edition)
        {
            ValidationResult result = this.validator.Validate(edition);

            if (!result.IsValid)
            {
                Log.Warn($"Validation failed for EditionID={edition.Id}: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }
        }
    }
}
