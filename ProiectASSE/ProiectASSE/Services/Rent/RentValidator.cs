// <copyright file="RentValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides validation rules for <see cref="Rent"/> entities.
    /// Ensures that rent data is structurally valid, logically consistent,
    /// and adheres to business constraints such as valid dates and required items.
    /// </summary>
    public class RentValidator : AbstractValidator<Rent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentValidator"/> class.
        /// Defines validation rules for rent properties.
        /// </summary>
        public RentValidator()
        {
            this.RuleFor(r => r.ReaderId)
                .GreaterThan(0)
                .WithMessage("ReaderId must be a valid positive number");

            this.RuleFor(r => r.BookCopies)
                .NotNull().WithMessage("BookCopies collection cannot be null")
                .Must(c => c != null && c.Count > 0)
                .WithMessage("At least one BookCopy must be included in a rent");

            this.RuleFor(r => r.StartDate)
                .LessThan(r => r.DueDate)
                .WithMessage("StartDate must be earlier than DueDate");

            this.RuleFor(r => r.ExtensionDaysTotal)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ExtensionDaysTotal cannot be negative");

            this.RuleFor(r => r.NumberOfExtensions)
                .GreaterThanOrEqualTo(0)
                .WithMessage("NumberOfExtensions cannot be negative");

            this.RuleFor(r => r.Status)
                .IsInEnum()
                .WithMessage("Invalid rent status");

            this.RuleFor(r => r.ReturnDate).Must(_ => true);

            this.RuleFor(r => r.ProcessedByEmployeeId).Must(_ => true);

            this.RuleFor(r => r.ProcessedByEmployee).Must(_ => true);

            this.RuleFor(r => r.Reader).Must(_ => true);
        }
    }
}
