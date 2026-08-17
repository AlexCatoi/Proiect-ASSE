// <copyright file="AuthorValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.AuthorService
{
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides validation rules for <see cref="Author"/> entities.
    /// Ensures that author data contains required fields and respects
    /// length constraints for names.
    /// </summary>
    public class AuthorValidator : AbstractValidator<Author>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorValidator"/> class.
        /// Defines validation rules for author properties.
        /// </summary>
        public AuthorValidator()
        {
            this.RuleFor(a => a.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50);

            this.RuleFor(a => a.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50);
        }
    }
}
