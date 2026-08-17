// <copyright file="ReaderValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.ReaderService
{
    using System;
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides validation rules for <see cref="Reader"/> entities.
    /// Ensures that reader information is complete, correctly formatted,
    /// and logically consistent with business requirements.
    /// </summary>
    public class ReaderValidator : AbstractValidator<Reader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderValidator"/> class.
        /// Defines validation rules for reader properties.
        /// </summary>
        public ReaderValidator()
        {
            this.RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50);

            this.RuleFor(r => r.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(50);

            this.RuleFor(r => r.Email)
                .EmailAddress().When(r => !string.IsNullOrWhiteSpace(r.Email))
                .WithMessage("Invalid email format");

            this.RuleFor(r => r.Phone)
                .Matches(@"^[0-9+\- ]+$").When(r => !string.IsNullOrWhiteSpace(r.Phone))
                .WithMessage("Invalid phone number");

            this.RuleFor(r => r)
                .Must(r => !string.IsNullOrWhiteSpace(r.Email) || !string.IsNullOrWhiteSpace(r.Phone))
                .WithMessage("At least one contact method (email or phone) is required");

            this.RuleFor(r => r.EnrollDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Enroll date cannot be in the future");
        }
    }
}
