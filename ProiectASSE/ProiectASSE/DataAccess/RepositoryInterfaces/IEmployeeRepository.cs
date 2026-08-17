// <copyright file="IEmployeeRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines data access operations for <see cref="Employee"/> entities.
    /// Implementations of this interface provide CRUD functionality
    /// and interaction with the underlying data storage.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Retrieves an employee by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the employee to retrieve.</param>
        /// <returns>
        /// The <see cref="Employee"/> with the specified identifier,
        /// or <c>null</c> if no matching employee is found.
        /// </returns>
        Employee GetById(int id);

        /// <summary>
        /// Retrieves all employees from the data source.
        /// </summary>
        /// <returns>A collection of all <see cref="Employee"/> entities.</returns>
        IEnumerable<Employee> GetAll();

        /// <summary>
        /// Adds a new employee to the data source.
        /// </summary>
        /// <param name="employee">The employee entity to add.</param>
        void Add(Employee employee);

        /// <summary>
        /// Updates an existing employee in the data source.
        /// </summary>
        /// <param name="employee">The employee entity with updated values.</param>
        void Update(Employee employee);

        /// <summary>
        /// Deletes an employee with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the employee to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Determines whether an email address already exists for another employee.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <param name="excludeId">
        /// An optional employee identifier to exclude from the check,
        /// typically used when updating an existing employee.
        /// </param>
        /// <returns>
        /// <c>true</c> if the email exists for another employee; otherwise, <c>false</c>.
        /// </returns>
        bool EmailExists(string email, int? excludeId = null);

        /// <summary>
        /// Determines whether a phone number already exists for another employee.
        /// </summary>
        /// <param name="phone">The phone number to check.</param>
        /// <param name="excludeId">
        /// An optional employee identifier to exclude from the check,
        /// typically used when updating an existing employee.
        /// </param>
        /// <returns>
        /// <c>true</c> if the phone number exists for another employee; otherwise, <c>false</c>.
        /// </returns>
        bool PhoneExists(string phone, int? excludeId = null);

        /// <summary>
        /// Persists all pending changes to the data source.
        /// </summary>
        void SaveChanges();
    }
}
