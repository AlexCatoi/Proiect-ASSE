// <copyright file="Category.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DomainModel.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a book category within the library system.
    /// Categories may be hierarchical, allowing parent–child relationships
    /// to model domains and subdomains.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent category, if any.
        /// A <c>null</c> value indicates that the category is a root category.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent category in the hierarchy.
        /// </summary>
        public Category Parent { get; set; }

        /// <summary>
        /// Gets or sets the collection of child categories.
        /// </summary>
        public ICollection<Category> Children { get; set; }

        /// <summary>
        /// Gets or sets the collection of books associated with this category.
        /// </summary>
        public ICollection<Book> Books { get; set; }
    }
}
