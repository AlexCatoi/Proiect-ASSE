using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.BookCopyService;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BookCopyValidatorTests
    {
        private BookCopyValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new BookCopyValidator();
        }

        private BookCopy CreateValidCopy()
        {
            return new BookCopy
            {
                Id = 1,
                BookId = 10,
                IsReadingRoomOnly = false,
                IsBorrowed = false,
                Rents = new List<Rent>()
            };
        }

        [TestMethod]
        public void Should_Have_Error_When_BookId_Is_Invalid()
        {
            var model = CreateValidCopy();
            model.BookId = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.BookId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_BookId_Is_Valid()
        {
            var model = CreateValidCopy();
            model.BookId = 5;

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(c => c.BookId);
        }

        [TestMethod]
        public void Should_Have_Error_When_Rents_Is_Null()
        {
            var model = CreateValidCopy();
            model.Rents = null;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Rents);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Rents_Is_Empty_List()
        {
            var model = CreateValidCopy();
            model.Rents = new List<Rent>();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(c => c.Rents);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = CreateValidCopy();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
