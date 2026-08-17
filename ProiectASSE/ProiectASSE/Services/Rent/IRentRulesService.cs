// <copyright file="IRentRulesService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business rules and validation logic for processing rent requests.
    /// Implementations of this interface ensure that readers, employees,
    /// and book copies comply with all borrowing constraints.
    /// </summary>
    public interface IRentRulesService
    {
        /// <summary>
        /// Validates a rent request based on the reader's identity,
        /// employee status, selected book copies, and associated book domains.
        /// </summary>
        /// <param name="readerId">The identifier of the reader requesting the rent.</param>
        /// <param name="isEmployee">
        /// Indicates whether the requester is an employee, which may affect borrowing rules.
        /// </param>
        /// <param name="copies">The list of book copies requested for borrowing.</param>
        /// <param name="bookDomains">
        /// The list of domains (categories) associated with the requested books.
        /// Used to enforce domain‑based borrowing restrictions.
        /// </param>
        void ValidateRentRequest(
            int readerId,
            bool isEmployee,
            List<BookCopy> copies,
            List<string> bookDomains);
    }
}
