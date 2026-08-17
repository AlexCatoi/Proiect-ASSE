// <copyright file="Employee.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    using System;

    /// <summary>
    /// Represents an employee of the library.
    /// Inherits from <see cref="Reader"/> since employees can also borrow books.
    /// </summary>
    public class Employee : Reader
    {
        /// <summary>
        /// Gets or sets the date when the employee was hired.
        /// </summary>
        public DateTime EmployDate { get; set; }
    }
}
