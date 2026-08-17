// <copyright file="EmployeeService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.EmployeeService
{
    using System.Collections.Generic;
    using FluentValidation;
    using FluentValidation.Results;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides business logic operations for managing <see cref="Employee"/> entities.
    /// This service handles validation, uniqueness checks, and delegates persistence
    /// operations to the repository layer.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// Logger instance for this service.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmployeeService));

        /// <summary>
        /// The repository used to access employee data.
        /// </summary>
        private readonly IEmployeeRepository repository;

        /// <summary>
        /// The validator used to validate <see cref="Employee"/> entities.
        /// </summary>
        private readonly IValidator<Employee> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="repository">The repository used for employee data access.</param>
        /// <param name="validator">The validator used to validate employee entities.</param>
        public EmployeeService(IEmployeeRepository repository, IValidator<Employee> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public Employee GetEmployee(int id)
        {
            return this.repository.GetById(id);
        }

        /// <inheritdoc/>
        public IEnumerable<Employee> GetAllEmployees()
        {
            return this.repository.GetAll();
        }

        /// <inheritdoc/>
        public void RegisterEmployee(Employee employee)
        {
            this.Validate(employee, isUpdate: false);

            this.repository.Add(employee);
            this.repository.SaveChanges();

            Log.Info($"Employee registered successfully: {employee.Name} (ID={employee.Id})");
        }

        /// <inheritdoc/>
        public void UpdateEmployee(Employee employee)
        {
            this.Validate(employee, isUpdate: true);

            this.repository.Update(employee);
            this.repository.SaveChanges();

            Log.Info($"Employee updated successfully: {employee.Name} (ID={employee.Id})");
        }

        /// <inheritdoc/>
        public void DeleteEmployee(int id)
        {
            this.repository.Delete(id);
            this.repository.SaveChanges();

            Log.Warn($"Employee deleted: ID={id}");
        }

        /// <summary>
        /// Validates an employee entity using the configured validator
        /// and ensures that email and phone values are unique.
        /// </summary>
        /// <param name="employee">The employee entity to validate.</param>
        /// <param name="isUpdate">
        /// Indicates whether the validation is performed during an update operation.
        /// </param>
        /// <exception cref="ValidationException">
        /// Thrown when validation fails or when email/phone uniqueness is violated.
        /// </exception>
        private void Validate(Employee employee, bool isUpdate)
        {
            ValidationResult result = this.validator.Validate(employee);

            if (!result.IsValid)
            {
                Log.Warn($"Validation failed for EmployeeID={employee.Id}: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }

            if (!string.IsNullOrWhiteSpace(employee.Email))
            {
                if (this.repository.EmailExists(employee.Email, isUpdate ? employee.Id : (int?)null))
                {
                    Log.Warn($"Email already exists for Employee: {employee.Email}");
                    throw new ValidationException("Email already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(employee.Phone))
            {
                if (this.repository.PhoneExists(employee.Phone, isUpdate ? employee.Id : (int?)null))
                {
                    Log.Warn($"Phone already exists for Employee: {employee.Phone}");
                    throw new ValidationException("Phone already exists");
                }
            }
        }
    }
}
