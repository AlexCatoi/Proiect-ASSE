// <copyright file="IEmployeeService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.EmployeeService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Defines business logic operations for managing <see cref="Employee"/> entities.
    /// Implementations of this interface handle validation, uniqueness checks,
    /// and interactions with the underlying data access layer.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves an employee by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the employee to retrieve.</param>
        /// <returns>
        /// The <see cref="Employee"/> with the specified identifier,
        /// or <c>null</c> if no matching employee is found.
        /// </returns>
        Employee GetEmployee(int id);

        /// <summary>
        /// Retrieves all employees from the system.
        /// </summary>
        /// <returns>A collection of all <see cref="Employee"/> entities.</returns>
        IEnumerable<Employee> GetAllEmployees();

        /// <summary>
        /// Registers a new employee after validation and uniqueness checks.
        /// </summary>
        /// <param name="employee">The employee entity to register.</param>
        void RegisterEmployee(Employee employee);

        /// <summary>
        /// Updates an existing employee after validation and uniqueness checks.
        /// </summary>
        /// <param name="employee">The employee entity with updated values.</param>
        void UpdateEmployee(Employee employee);

        /// <summary>
        /// Deletes an employee with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the employee to delete.</param>
        void DeleteEmployee(int id);
    }
}
