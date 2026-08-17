// <copyright file="RentService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;
    using ProiectASSE.Services.BookCopyService;
    using ProiectASSE.Services.ReaderService;

    /// <summary>
    /// Provides business logic operations for managing <see cref="Rent"/> entities.
    /// This service handles rent creation, extensions, returns, and enforces
    /// borrowing rules through <see cref="IRentRulesService"/>.
    /// </summary>
    public class RentService : IRentService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RentService));

        private readonly IRentRepository repo;
        private readonly IValidator<Rent> validator;
        private readonly IRentRulesService rules;
        private readonly IBookCopyService copyService;
        private readonly IReaderService readerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for rent data access.</param>
        /// <param name="validator">The validator used to validate rent entities.</param>
        /// <param name="rules">The service enforcing rent rules.</param>
        /// <param name="copyService">The service used to retrieve book copies.</param>
        /// <param name="readerService">The service used to retrieve reader information.</param>
        public RentService(
            IRentRepository repo,
            IValidator<Rent> validator,
            IRentRulesService rules,
            IBookCopyService copyService,
            IReaderService readerService)
        {
            this.repo = repo;
            this.validator = validator;
            this.rules = rules;
            this.copyService = copyService;
            this.readerService = readerService;
        }

        /// <inheritdoc/>
        public Rent CreateRent(int readerId, List<int> copyIds, int? employeeId = null)
        {
            var reader = this.readerService.GetReader(readerId);
            if (reader == null)
            {
                Log.Warn($"Rent creation failed: ReaderID={readerId} not found.");
                throw new Exception("Reader not found.");
            }

            bool isEmployee = reader is Employee;

            var copies = copyIds
                .Select(id => this.copyService.GetCopy(id))
                .ToList();

            if (copies.Any(c => c == null))
            {
                Log.Warn($"Rent creation failed: One or more copies do not exist. CopyIDs=[{string.Join(", ", copyIds)}]");
                throw new Exception("One or more copies do not exist.");
            }

            foreach (var copy in copies)
            {
                if (copy.IsReadingRoomOnly)
                {
                    Log.Warn($"Rent denied: CopyID={copy.Id} is reading-room only.");
                    throw new Exception($"Copy {copy.Id} is reading-room only.");
                }

                if (copy.IsBorrowed)
                {
                    Log.Warn($"Rent denied: CopyID={copy.Id} is already borrowed.");
                    throw new Exception($"Copy {copy.Id} is already borrowed.");
                }
            }

            var domains = copies
                .SelectMany(c => c.Book.Categories)
                .Select(cat => cat.Name)
                .ToList();

            try
            {
                this.rules.ValidateRentRequest(readerId, isEmployee, copies, domains);
            }
            catch (Exception ex)
            {
                Log.Warn($"Rent rule validation failed for ReaderID={readerId}: {ex.Message}");
                throw;
            }

            var rent = new Rent
            {
                ReaderId = readerId,
                ProcessedByEmployeeId = employeeId,
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                Status = RentStatus.ACTIVE,
                BookCopies = copies,
                NumberOfExtensions = 0,
                ExtensionDaysTotal = 0,
            };

            var result = this.validator.Validate(rent);
            if (!result.IsValid)
            {
                Log.Warn($"Rent validation failed: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }

            foreach (var copy in copies)
            {
                copy.IsBorrowed = true;
            }

            this.repo.Add(rent);
            this.repo.SaveChanges();

            Log.Info($"Rent created successfully: RentID={rent.Id}, ReaderID={readerId}, Copies=[{string.Join(", ", copyIds)}]");

            return rent;
        }

        /// <inheritdoc/>
        public void ExtendRent(int rentId, int days)
        {
            var rent = this.repo.GetById(rentId);
            if (rent == null)
            {
                Log.Warn($"Rent extension failed: RentID={rentId} not found.");
                throw new Exception("Rent not found.");
            }

            rent.DueDate = rent.DueDate.AddDays(days);
            rent.NumberOfExtensions++;
            rent.ExtensionDaysTotal += days;

            this.repo.Update(rent);
            this.repo.SaveChanges();

            Log.Info($"Rent extended: RentID={rentId}, Days={days}, NewDueDate={rent.DueDate}");
        }

        /// <inheritdoc/>
        public void ReturnRent(int rentId)
        {
            var rent = this.repo.GetById(rentId);
            if (rent == null)
            {
                Log.Warn($"Rent return failed: RentID={rentId} not found.");
                throw new Exception("Rent not found.");
            }

            rent.Status = RentStatus.RETURNED;

            foreach (var copy in rent.BookCopies)
            {
                copy.IsBorrowed = false;
            }

            this.repo.Update(rent);
            this.repo.SaveChanges();

            Log.Info($"Rent returned successfully: RentID={rentId}");
        }

        /// <inheritdoc/>
        public IEnumerable<Rent> GetActiveRents(int readerId)
        {
            var rents = this.repo.GetActiveRentsForReader(readerId);

            Log.Info($"Retrieved {rents.Count()} active rents for ReaderID={readerId}");

            return rents;
        }
    }
}