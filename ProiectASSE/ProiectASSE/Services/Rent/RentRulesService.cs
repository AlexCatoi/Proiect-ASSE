// <copyright file="RentRulesService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides rule‑based validation for rent requests.
    /// This service enforces borrowing limits, domain restrictions,
    /// cooldown periods, daily limits, and extension constraints.
    /// </summary>
    public class RentRulesService : IRentRulesService
    {
        /// <summary>
        /// Service used to retrieve historical rent data for a reader.
        /// </summary>
        private readonly IRentHistoryService history;

        /// <summary>
        /// Service used to retrieve configurable borrowing limits.
        /// </summary>
        private readonly IRentLimitService limits;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentRulesService"/> class.
        /// </summary>
        /// <param name="history">The service providing rent history data.</param>
        /// <param name="limits">The service providing borrowing limit values.</param>
        public RentRulesService(
            IRentHistoryService history,
            IRentLimitService limits)
        {
            this.history = history;
            this.limits = limits;
        }

        /// <inheritdoc/>
        public void ValidateRentRequest(
            int readerId,
            bool isEmployee,
            List<BookCopy> copies,
            List<string> bookDomains)
        {
            this.ValidateMaxBooksPerRequest(isEmployee, copies);
            this.ValidateDistinctDomainsIfNeeded(copies, bookDomains);
            this.ValidateMaxBooksPerPeriod(readerId, isEmployee, copies);
            this.ValidateMaxBooksPerDomain(readerId, isEmployee, copies, bookDomains);
            this.ValidateDeltaRule(readerId, isEmployee, copies);
            this.ValidateDailyLimit(readerId, isEmployee, copies);
            this.ValidateEmployeeProcessingLimit(isEmployee, copies);
            this.ValidateExtensionsLimit(readerId, isEmployee);
        }

        /// <summary>
        /// Ensures that the number of books in a single request does not exceed the configured limit.
        /// </summary>
        private void ValidateMaxBooksPerRequest(bool isEmployee, List<BookCopy> copies)
        {
            int limit = this.limits.GetC(isEmployee);

            if (copies.Count > limit)
            {
                throw new Exception($"Cannot borrow more than {limit} books in one request.");
            }
        }

        /// <summary>
        /// Ensures that when borrowing 3 or more books, they span at least two distinct domains.
        /// </summary>
        private void ValidateDistinctDomainsIfNeeded(List<BookCopy> copies, List<string> domains)
        {
            if (copies.Count < 3)
            {
                return;
            }

            if (domains.Distinct().Count() < 2)
            {
                throw new Exception("If borrowing 3 or more books, they must belong to at least 2 distinct domains.");
            }
        }

        /// <summary>
        /// Ensures that the reader does not exceed the maximum number of books allowed
        /// within a configurable time period.
        /// </summary>
        private void ValidateMaxBooksPerPeriod(int readerId, bool isEmployee, List<BookCopy> copies)
        {
            int nmc = this.limits.GetNMC(isEmployee);
            int per = this.limits.GetPER(isEmployee);

            var periodStart = DateTime.Now.AddDays(-per);
            var rents = this.history.GetRentsInPeriod(readerId, periodStart, DateTime.Now);

            int alreadyBorrowed = rents.Sum(r => r.BookCopies.Count);

            if (alreadyBorrowed + copies.Count > nmc)
            {
                throw new Exception($"Cannot borrow more than {nmc} books in a {per}-day period.");
            }
        }

        /// <summary>
        /// Ensures that the reader does not exceed the maximum number of books
        /// allowed per domain within a configurable number of months.
        /// </summary>
        private void ValidateMaxBooksPerDomain(int readerId, bool isEmployee, List<BookCopy> copies, List<string> domains)
        {
            int d = this.limits.GetD(isEmployee);
            int l = this.limits.GetL(isEmployee);

            var periodStart = DateTime.Now.AddMonths(-l);
            var rents = this.history.GetRentsInPeriod(readerId, periodStart, DateTime.Now);

            foreach (var domain in domains.Distinct())
            {
                int alreadyBorrowed = rents
                    .SelectMany(r => r.BookCopies)
                    .Count(copy => copy.Book.Categories
                        .Any(cat => cat.Name == domain));

                int newCount = copies
                    .Count(copy => copy.Book.Categories
                        .Any(cat => cat.Name == domain));

                if (alreadyBorrowed + newCount > d)
                {
                    throw new Exception(
                        $"Cannot borrow more than {d} books from domain '{domain}' in the last {l} months.");
                }
            }
        }

        /// <summary>
        /// Ensures that the reader does not borrow the same book again
        /// within a configured cooldown period.
        /// </summary>
        private void ValidateDeltaRule(int readerId, bool isEmployee, List<BookCopy> copies)
        {
            int delta = this.limits.GetDELTA(isEmployee);

            foreach (var copy in copies)
            {
                var rents = this.history.GetRentsForBook(readerId, copy.BookId);

                if (rents.Any(r => (DateTime.Now - r.StartDate).TotalDays < delta))
                {
                    throw new Exception($"Cannot borrow the same book again within {delta} days.");
                }
            }
        }

        /// <summary>
        /// Ensures that non‑employees do not exceed the daily borrowing limit.
        /// </summary>
        public void ValidateDailyLimit(int readerId, bool isEmployee, List<BookCopy> copies)
        {
            if (isEmployee)
            {
                return;
            }

            int ncz = this.limits.GetNCZ(false);

            var todayRents = this.history.GetRentsForDay(readerId, DateTime.Today);
            int alreadyToday = todayRents.Sum(r => r.BookCopies.Count);

            if (alreadyToday + copies.Count > ncz)
            {
                throw new Exception($"Cannot borrow more than {ncz} books in a single day.");
            }
        }

        /// <summary>
        /// Ensures that employees do not process more books in a single request
        /// than the configured processing limit.
        /// </summary>
        private void ValidateEmployeeProcessingLimit(bool isEmployee, List<BookCopy> copies)
        {
            if (!isEmployee)
            {
                return;
            }

            int persimp = this.limits.GetPERSIMP();

            if (copies.Count > persimp)
            {
                throw new Exception($"Employee cannot process more than {persimp} books in a single request.");
            }
        }

        /// <summary>
        /// Ensures that the reader does not exceed the maximum number of allowed
        /// extensions within the last three months.
        /// </summary>
        private void ValidateExtensionsLimit(int readerId, bool isEmployee)
        {
            int lim = this.limits.GetLIM(isEmployee);

            var extensions = this.history.GetExtensionsInLast3Months(readerId)
                .Sum(r => r.NumberOfExtensions);

            if (extensions > lim)
            {
                throw new Exception($"Cannot exceed {lim} extensions in the last 3 months.");
            }
        }
    }
}
