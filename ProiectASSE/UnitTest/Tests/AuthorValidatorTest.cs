using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.AuthorService;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AuthorValidatorTests
    {
        private AuthorValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new AuthorValidator();
        }

        private Author CreateValidAuthor()
        {
            return new Author
            {
                FirstName = "John",
                LastName = "Doe",
                Books = new List<Book>()
            };
        }

        [TestMethod]
        public void Should_Have_Error_When_FirstName_Empty()
        {
            var model = CreateValidAuthor();
            model.FirstName = "";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(a => a.FirstName);
        }

        [TestMethod]
        public void Should_Have_Error_When_LastName_Empty()
        {
            var model = CreateValidAuthor();
            model.LastName = "";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(a => a.LastName);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Valid()
        {
            var model = CreateValidAuthor();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
