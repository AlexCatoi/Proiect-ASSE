// <copyright file="IRentService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business logic operations for managing <see cref="Rent"/> entities.
    /// Implementations of this interface handle rent creation, extensions,
    /// returns, and retrieval of active rents for a reader.
    /// </summary>
    public interface IRentService
    {
        /// <summary>
        /// Creates a new rent for the specified reader and book copies.
        /// </summary>
        /// <param name="readerId">The identifier of the reader requesting the rent.</param>
        /// <param name="copyIds">The identifiers of the book copies to be rented.</param>
        /// <param name="employeeId">
        /// Optional identifier of the employee processing the rent.
        /// If <c>null</c>, the rent is processed as a standard reader request.
        /// </param>
        /// <returns>
        /// The newly created <see cref="Rent"/> entity.
        /// </returns>
        Rent CreateRent(int readerId, List<int> copyIds, int? employeeId = null);

        /// <summary>
        /// Extends the duration of an existing rent by the specified number of days.
        /// </summary>
        /// <param name="rentId">The identifier of the rent to extend.</param>
        /// <param name="days">The number of days to extend the rent.</param>
        void ExtendRent(int rentId, int days);

        /// <summary>
        /// Marks a rent as returned and updates the status of its associated book copies.
        /// </summary>
        /// <param name="rentId">The identifier of the rent to return.</param>
        void ReturnRent(int rentId);

        /// <summary>
        /// Retrieves all active (non‑returned) rents for the specified reader.
        /// </summary>
        /// <param name="readerId">The identifier of the reader whose active rents are requested.</param>
        /// <returns>A collection of active <see cref="Rent"/> entities.</returns>
        IEnumerable<Rent> GetActiveRents(int readerId);
    }
}
