using System;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.CategoryService;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CategoryValidatorTests
    {
        private CategoryValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new CategoryValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new Category { Name = "" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [TestMethod]
        public void Should_Have_Error_When_Name_Too_Long()
        {
            var model = new Category { Name = new string('a', 51) };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [TestMethod]
        public void Should_Have_Error_When_Category_Is_Its_Own_Parent()
        {
            var model = new Category
            {
                Id = 5,
                ParentId = 5,
                Name = "Test"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Valid()
        {
            var model = new Category
            {
                Id = 1,
                ParentId = 2,
                Name = "Science"
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Parent_Set()
        {
            var model = new Category
            {
                Name = "Test",
                Parent = new Category { Id = 10, Name = "Parent" }
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(c => c.Parent);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Children_Set()
        {
            var model = new Category
            {
                Name = "Test",
                Children = new List<Category>
        {
            new Category { Id = 2, Name = "Child" }
        }
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(c => c.Children);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Books_Set()
        {
            var model = new Category
            {
                Name = "Test",
                Books = new List<Book>
        {
            new Book { Id = 1, Title = "Book A" }
        }
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(c => c.Books);
        }

    }
}
