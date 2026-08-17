// <copyright file="CategoryService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.CategoryService
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using log4net;
    using ProiectASSE.DataAccess.Repository;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides business logic operations for managing <see cref="Category"/> entities.
    /// This service handles validation, hierarchical consistency checks,
    /// and delegates persistence operations to the repository layer.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        /// <summary>
        /// Logger instance for this service.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryService));

        /// <summary>
        /// The repository used to access category data.
        /// </summary>
        private readonly ICategoryRepository repo;

        /// <summary>
        /// The validator used to validate <see cref="Category"/> entities.
        /// </summary>
        private readonly IValidator<Category> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for category data access.</param>
        /// <param name="validator">The validator used to validate category entities.</param>
        public CategoryService(ICategoryRepository repo, IValidator<Category> validator)
        {
            this.repo = repo;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public Category GetCategory(int id) => this.repo.GetById(id);

        /// <inheritdoc/>
        public IEnumerable<Category> GetAllCategories() => this.repo.GetAll();

        /// <inheritdoc/>
        public void AddCategory(Category category)
        {
            this.ValidateCategoryTree(category);

            this.repo.Add(category);
            this.repo.SaveChanges();

            Log.Info($"Category added successfully: '{category.Name}' (ID={category.Id})");
        }

        /// <inheritdoc/>
        public void UpdateCategory(Category category)
        {
            this.ValidateCategoryTree(category);

            this.repo.Update(category);
            this.repo.SaveChanges();

            Log.Info($"Category updated successfully: '{category.Name}' (ID={category.Id})");
        }

        /// <inheritdoc/>
        public void DeleteCategory(int id)
        {
            this.repo.Delete(id);
            this.repo.SaveChanges();

            Log.Warn($"Category deleted: ID={id}");
        }

        /// <inheritdoc/>
        public bool IsAncestor(int ancestorId, int descendantId)
        {
            var current = this.repo.GetById(descendantId);

            while (current?.ParentId != null)
            {
                if (current.ParentId == ancestorId)
                {
                    Log.Info($"Category {ancestorId} is an ancestor of {descendantId}");
                    return true;
                }

                current = this.repo.GetById(current.ParentId.Value);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsDescendant(int descendantId, int ancestorId)
            => this.IsAncestor(ancestorId, descendantId);

        /// <inheritdoc/>
        public IEnumerable<Category> GetAncestors(int categoryId)
        {
            var result = new List<Category>();
            var current = this.repo.GetById(categoryId);

            while (current?.ParentId != null)
            {
                current = this.repo.GetById(current.ParentId.Value);
                if (current != null)
                {
                    result.Add(current);
                }
            }

            Log.Info($"Retrieved {result.Count} ancestors for CategoryID={categoryId}");
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<Category> GetDescendants(int categoryId)
        {
            var all = this.repo.GetAll().ToList();
            var result = new List<Category>();

            void DFS(int id)
            {
                foreach (var c in all.Where(x => x.ParentId == id))
                {
                    result.Add(c);
                    DFS(c.Id);
                }
            }

            DFS(categoryId);

            Log.Info($"Retrieved {result.Count} descendants for CategoryID={categoryId}");
            return result;
        }

        /// <inheritdoc/>
        public void ValidateCategoryTree(Category category)
        {
            ValidationResult result = this.validator.Validate(category);

            if (!result.IsValid)
            {
                Log.Warn($"Validation failed for CategoryID={category.Id}: {string.Join("; ", result.Errors)}");
                throw new ValidationException(result.Errors);
            }

            if (category.ParentId != null)
            {
                if (this.IsAncestor(category.Id, category.ParentId.Value))
                {
                    Log.Warn($"Cycle detected: CategoryID={category.Id} cannot have ParentID={category.ParentId}");
                    throw new ValidationException("Cannot create a cycle in category tree");
                }
            }
        }
    }
}
