// <copyright file="EditionValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.EditionService
{
    using System;
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides validation rules for <see cref="Edition"/> entities.
    /// Ensures that edition metadata such as publisher, year, page count,
    /// and associated book information is complete and logically valid.
    /// </summary>
    public class EditionValidator : AbstractValidator<Edition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditionValidator"/> class.
        /// Defines validation rules for edition properties.
        /// </summary>
        public EditionValidator()
        {
            this.RuleFor(e => e.Publisher)
                .NotEmpty().WithMessage("Publisher is required")
                .MaximumLength(100);

            this.RuleFor(e => e.Year)
                .GreaterThan(0)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Year must be valid");

            this.RuleFor(e => e.Pages)
                .GreaterThan(0)
                .WithMessage("Pages must be greater than 0");

            this.RuleFor(e => e.BookType)
                .NotEmpty().WithMessage("BookType is required");

            this.RuleFor(e => e.BookId)
                .GreaterThan(0)
                .WithMessage("Edition must be linked to a valid Book");
        }
    }
}
