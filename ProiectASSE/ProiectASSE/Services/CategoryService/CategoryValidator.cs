// <copyright file="CategoryValidator.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.CategoryService
{
    using FluentValidation;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides validation rules for <see cref="Category"/> entities.
    /// Ensures that category data is structurally valid and prevents
    /// invalid hierarchical relationships.
    /// </summary>
    public class CategoryValidator : AbstractValidator<Category>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryValidator"/> class.
        /// Defines validation rules for category properties.
        /// </summary>
        public CategoryValidator()
        {
            this.RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(50);

            this.RuleFor(c => c)
                .Must(c => c.ParentId != c.Id)
                .WithMessage("A category cannot be its own parent");

            this.RuleFor(c => c.Parent).Must(_ => true);

            this.RuleFor(c => c.Children).Must(_ => true);

            this.RuleFor(c => c.Books).Must(_ => true);
        }
    }
}
