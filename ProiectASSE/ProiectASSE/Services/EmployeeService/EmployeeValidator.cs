// <copyright file="EmployeeValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.EmployeeService
{
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;
    using ProiectASSE.Services.ReaderService;

    /// <summary>
    /// Provides validation rules for <see cref="Employee"/> entities.
    /// Extends the <see cref="ReaderValidator"/> to validate shared fields
    /// and adds employee‑specific validation rules.
    /// </summary>
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeValidator"/> class.
        /// Defines validation rules for employee properties.
        /// </summary>
        public EmployeeValidator()
        {
            this.Include(new ReaderValidator());

            this.RuleFor(e => e.EmployDate)
                .GreaterThanOrEqualTo(e => e.EnrollDate)
                .WithMessage("Employ date cannot be before enroll date");
        }
    }
}
