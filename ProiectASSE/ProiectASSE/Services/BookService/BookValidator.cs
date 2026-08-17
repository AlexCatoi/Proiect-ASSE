// <copyright file="BookValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.BookService
{
    using System.Configuration;
    using System.Linq;
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides validation rules for <see cref="Book"/> entities.
    /// Ensures that books contain required metadata, have valid relationships,
    /// and respect configurable constraints such as maximum category count.
    /// </summary>
    public class BookValidator : AbstractValidator<Book>
    {
        /// <summary>
        /// The maximum number of categories a book may belong to.
        /// Loaded from application configuration, with a default fallback.
        /// </summary>
        private readonly int maxCategories;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookValidator"/> class.
        /// Defines validation rules for book properties.
        /// </summary>
        public BookValidator()
        {
            this.maxCategories = int.TryParse(ConfigurationManager.AppSettings["Domenii"], out int val)
                ? val
                : 5;

            this.RuleFor(b => b.Title)
                .NotEmpty().WithMessage("Book title is required")
                .MaximumLength(100);

            this.RuleFor(b => b.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(100);

            this.RuleFor(b => b.Categories)
                .NotNull().WithMessage("Book must have at least one category")
                .Must(c => c != null && c.Any())
                .WithMessage("Book must have at least one category")
                .Must(c => c.Count <= this.maxCategories)
                .WithMessage(b => $"Book cannot have more than {this.maxCategories} categories");

            this.RuleFor(b => b.Authors)
                .NotNull().WithMessage("Book must have at least one author")
                .Must(a => a != null && a.Any())
                .WithMessage("Book must have at least one author");

            this.RuleFor(b => b.Editions)
                .NotNull().WithMessage("Book must have at least one edition")
                .Must(e => e != null && e.Any())
                .WithMessage("Book must have at least one edition");

            this.RuleFor(b => b.Copies)
                .NotNull().WithMessage("Book must have at least one copy")
                .Must(c => c != null && c.Any())
                .WithMessage("Book must have at least one copy");
        }
    }
}
