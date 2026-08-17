// <copyright file="EmployeeRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="Employee"/> entities.
    /// Employees are stored in the same table as readers, therefore queries
    /// filter the <see cref="Reader"/> set by type.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        /// <summary>
        /// The database context used to access employee data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public EmployeeRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public Employee GetById(int id)
        {
            return this.context.Readers
                .OfType<Employee>()
                .FirstOrDefault(e => e.Id == id);
        }

        /// <inheritdoc/>
        public IEnumerable<Employee> GetAll()
        {
            return this.context.Readers
                .OfType<Employee>()
                .ToList();
        }

        /// <inheritdoc/>
        public void Add(Employee employee)
        {
            this.context.Readers.Add(employee);
        }

        /// <inheritdoc/>
        public void Update(Employee employee)
        {
            var entry = this.context.Entry(employee);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.Readers.Attach(employee);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            var employee = this.context.Readers
                .OfType<Employee>()
                .FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                this.context.Readers.Remove(employee);
            }
        }

        /// <inheritdoc/>
        public bool EmailExists(string email, int? excludeId = null)
        {
            return this.context.Readers
                .OfType<Employee>()
                .Any(e => e.Email == email && (!excludeId.HasValue || e.Id != excludeId.Value));
        }

        /// <inheritdoc/>
        public bool PhoneExists(string phone, int? excludeId = null)
        {
            return this.context.Readers
                .OfType<Employee>()
                .Any(e => e.Phone == phone && (!excludeId.HasValue || e.Id != excludeId.Value));
        }

        /// <inheritdoc/>
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}
