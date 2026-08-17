// <copyright file="IRentLimitService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    /// <summary>
    /// Provides access to configurable borrowing limits and rule parameters
    /// used throughout the rent validation process.
    /// Implementations of this interface supply values that differ for
    /// employees and non‑employees where applicable.
    /// </summary>
    public interface IRentLimitService
    {
        /// <summary>
        /// Gets the maximum number of books a reader may borrow within a defined period.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The maximum number of books allowed in the period.</returns>
        int GetNMC(bool isEmployee);

        /// <summary>
        /// Gets the number of days that define the borrowing period
        /// used together with <see cref="GetNMC(bool)"/>.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The number of days in the borrowing period.</returns>
        int GetPER(bool isEmployee);

        /// <summary>
        /// Gets the maximum number of books allowed in a single rent request.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The maximum number of books per request.</returns>
        int GetC(bool isEmployee);

        /// <summary>
        /// Gets the maximum number of books allowed from the same domain
        /// within a defined number of months.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The maximum number of books per domain.</returns>
        int GetD(bool isEmployee);

        /// <summary>
        /// Gets the number of months used to evaluate domain‑based borrowing limits.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The number of months in the evaluation window.</returns>
        int GetL(bool isEmployee);

        /// <summary>
        /// Gets the maximum number of allowed extensions within the last three months.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The maximum number of extensions allowed.</returns>
        int GetLIM(bool isEmployee);

        /// <summary>
        /// Gets the minimum number of days that must pass before a reader
        /// may borrow the same book again.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The cooldown period in days.</returns>
        int GetDELTA(bool isEmployee);

        /// <summary>
        /// Gets the maximum number of books a non‑employee may borrow in a single day.
        /// </summary>
        /// <param name="isEmployee">Indicates whether the requester is an employee.</param>
        /// <returns>The daily borrowing limit.</returns>
        int GetNCZ(bool isEmployee);

        /// <summary>
        /// Gets the maximum number of books an employee may process
        /// in a single rent request.
        /// </summary>
        /// <returns>The processing limit for employees.</returns>
        int GetPERSIMP();
    }
}
