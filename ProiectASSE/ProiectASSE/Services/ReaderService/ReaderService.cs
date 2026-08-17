// <copyright file="ReaderService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.ReaderService
{
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Results;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides business logic operations for managing <see cref="Reader"/> entities.
    /// This service handles validation, uniqueness checks, and delegates persistence
    /// operations to the repository layer.
    /// </summary>
    public class ReaderService : IReaderService
    {
        /// <summary>
        /// Logger instance for this service.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReaderService));

        /// <summary>
        /// The repository used to access reader data.
        /// </summary>
        private readonly IReaderRepository repository;

        /// <summary>
        /// The validator used to validate <see cref="Reader"/> entities.
        /// </summary>
        private readonly IValidator<Reader> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderService"/> class.
        /// </summary>
        /// <param name="repository">The repository used for reader data access.</param>
        /// <param name="validator">The validator used to validate reader entities.</param>
        public ReaderService(IReaderRepository repository, IValidator<Reader> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public Reader GetReader(int id)
        {
            return this.repository.GetById(id);
        }

        /// <inheritdoc/>
        public IEnumerable<Reader> GetAllReaders()
        {
            return this.repository.GetAll();
        }

        /// <inheritdoc/>
        public void RegisterReader(Reader reader)
        {
            this.Validate(reader, isUpdate: false);

            this.repository.Add(reader);
            this.repository.SaveChanges();

            Log.Info($"Reader registered successfully: {reader.Name} (ID={reader.Id})");
        }

        /// <inheritdoc/>
        public void UpdateReader(Reader reader)
        {
            this.Validate(reader, isUpdate: true);

            this.repository.Update(reader);
            this.repository.SaveChanges();

            Log.Info($"Reader updated successfully: {reader.Name} (ID={reader.Id})");
        }

        /// <inheritdoc/>
        public void DeleteReader(int id)
        {
            this.repository.Delete(id);
            this.repository.SaveChanges();

            Log.Warn($"Reader deleted: ID={id}");
        }

        /// <summary>
        /// Validates a reader entity using the configured validator
        /// and ensures that email and phone values are unique.
        /// </summary>
        private void Validate(Reader reader, bool isUpdate)
        {
            ValidationResult result = this.validator.Validate(reader);

            if (!result.IsValid)
            {
                Log.Warn($"Validation failed for reader ID={reader.Id}: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }

            if (!string.IsNullOrWhiteSpace(reader.Email))
            {
                if (this.repository.EmailExists(reader.Email, isUpdate ? reader.Id : (int?)null))
                {
                    Log.Warn($"Email already exists: {reader.Email}");
                    throw new ValidationException("Email already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(reader.Phone))
            {
                if (this.repository.PhoneExists(reader.Phone, isUpdate ? reader.Id : (int?)null))
                {
                    Log.Warn($"Phone already exists: {reader.Phone}");
                    throw new ValidationException("Phone already exists");
                }
            }
        }
    }
}
