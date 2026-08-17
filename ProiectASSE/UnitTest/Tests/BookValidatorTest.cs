using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.BookService;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BookValidatorTests
    {
        private BookValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new BookValidator();
        }
        private Book CreateValidBook()
        {
            return new Book
            {
                Title = "Valid",
                Description = "Valid",
                Categories = new List<Category> { new Category() },
                Authors = new List<Author> { new Author() },
                Editions = new List<Edition> { new Edition() },
                Copies = new List<BookCopy> { new BookCopy() }
            };
        }


        [TestMethod]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            var model = CreateValidBook();
            model.Title = "";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Title);
        }


        [TestMethod]
        public void Should_Have_Error_When_Title_Too_Long()
        {
            var model = CreateValidBook();
            model.Title = new string('a', 101);

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Title);
        }


        [TestMethod]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var model = CreateValidBook();
            model.Description = "";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Description);
        }


        [TestMethod]
        public void Should_Have_Error_When_Description_Too_Long()
        {
            var model = CreateValidBook();
            model.Description = new string('a', 101);

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Description);
        }


        [TestMethod]
        public void Should_Have_Error_When_No_Categories()
        {
            var model = new Book { Categories = new List<Category>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(b => b.Categories);
        }

        [TestMethod]
        public void Should_Have_Error_When_Too_Many_Categories()
        {
            var model = new Book
            {
                Categories = new List<Category>
                {
                    new Category(), new Category(), new Category(),
                    new Category(), new Category(), new Category()
                }
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(b => b.Categories);
        }

        [TestMethod]
        public void Should_Have_Error_When_No_Authors()
        {
            var model = CreateValidBook();
            model.Authors = new List<Author>();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Authors);
        }


        [TestMethod]
        public void Should_Have_Error_When_No_Editions()
        {
            var model = CreateValidBook();
            model.Editions = new List<Edition>();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Editions);
        }


        [TestMethod]
        public void Should_Have_Error_When_No_Copies()
        {
            var model = CreateValidBook();
            model.Copies = new List<BookCopy>();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Copies);
        }


        [TestMethod]
        public void Should_Not_Have_Error_When_Valid()
        {
            var model = new Book
            {
                Title = "Valid",
                Description = "Valid",
                Categories = new List<Category> { new Category() },
                Authors = new List<Author> { new Author() },
                Editions = new List<Edition> { new Edition() },
                Copies = new List<BookCopy> { new BookCopy() }
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
