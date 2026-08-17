// <copyright file="BookCopyValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.BookCopyService
{
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides validation rules for <see cref="BookCopy"/> entities.
    /// Ensures that each copy is associated with a valid book and
    /// that required navigation properties are properly initialized.
    /// </summary>
    public class BookCopyValidator : AbstractValidator<BookCopy>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookCopyValidator"/> class.
        /// Defines validation rules for book copy properties.
        /// </summary>
        public BookCopyValidator()
        {
            this.RuleFor(c => c.BookId)
                .GreaterThan(0)
                .WithMessage("BookCopy must be linked to a valid Book");

            this.RuleFor(c => c.Rents)
                .NotNull()
                .WithMessage("Rents collection must not be null");
        }
    }
}
