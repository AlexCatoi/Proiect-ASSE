using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.EditionService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EditionValidatorTests
    {
        private EditionValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new EditionValidator();
        }

        private Edition CreateValidEdition()
        {
            return new Edition
            {
                Publisher = "Test",
                Year = 2020,
                Pages = 100,
                BookType = "Hardcover",
                BookId = 10
            };
        }

        [TestMethod]
        public void Should_Have_Error_When_Publisher_Is_Empty()
        {
            var model = CreateValidEdition();
            model.Publisher = "";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.Publisher);
        }

        [TestMethod]
        public void Should_Have_Error_When_Year_Invalid()
        {
            var model = CreateValidEdition();
            model.Year = -1;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.Year);
        }

        [TestMethod]
        public void Should_Have_Error_When_Pages_Invalid()
        {
            var model = CreateValidEdition();
            model.Pages = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.Pages);
        }

        [TestMethod]
        public void Should_Have_Error_When_BookType_Empty()
        {
            var model = CreateValidEdition();
            model.BookType = "";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.BookType);
        }

        [TestMethod]
        public void Should_Have_Error_When_BookId_Invalid()
        {
            var model = CreateValidEdition();
            model.BookId = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.BookId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Valid()
        {
            var model = CreateValidEdition();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
