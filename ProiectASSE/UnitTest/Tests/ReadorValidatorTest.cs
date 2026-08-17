using System;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.ReaderService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReaderValidatorTests
    {
        private ReaderValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new ReaderValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new Reader { Name = "" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.Name);
        }

        [TestMethod]
        public void Should_Have_Error_When_Address_Is_Empty()
        {
            var model = new Reader { Address = "" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.Address);
        }

        [TestMethod]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new Reader { Email = "invalid-email" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.Email);
        }

        [TestMethod]
        public void Should_Have_Error_When_Phone_Is_Invalid()
        {
            var model = new Reader { Phone = "abc123" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.Phone);
        }

        [TestMethod]
        public void Should_Have_Error_When_No_Email_And_No_Phone()
        {
            var model = new Reader
            {
                Name = "John",
                Address = "Street",
                Email = "",
                Phone = ""
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r);
        }

        [TestMethod]
        public void Should_Have_Error_When_EnrollDate_In_Future()
        {
            var model = new Reader
            {
                EnrollDate = DateTime.Now.AddDays(1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.EnrollDate);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reader_Is_Valid()
        {
            var model = new Reader
            {
                Name = "John",
                Address = "Street",
                Email = "john@test.com",
                Phone = "12345",
                EnrollDate = DateTime.Now.AddSeconds(-1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
