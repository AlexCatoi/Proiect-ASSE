using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.RentService;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RentValidatorTests
    {
        private RentValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new RentValidator();
        }

        private Rent CreateValidRent()
        {
            return new Rent
            {
                ReaderId = 10,
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                ExtensionDaysTotal = 0,
                NumberOfExtensions = 0,
                Status = RentStatus.ACTIVE,
                BookCopies = new List<BookCopy>
                {
                    new BookCopy { Id = 1, BookId = 5 }
                }
            };
        }

        [TestMethod]
        public void Should_Have_Error_When_ReaderId_Invalid()
        {
            var model = CreateValidRent();
            model.ReaderId = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.ReaderId);
        }

        [TestMethod]
        public void Should_Have_Error_When_BookCopies_Null()
        {
            var model = CreateValidRent();
            model.BookCopies = null;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.BookCopies);
        }

        [TestMethod]
        public void Should_Have_Error_When_BookCopies_Empty()
        {
            var model = CreateValidRent();
            model.BookCopies = new List<BookCopy>();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.BookCopies);
        }

        [TestMethod]
        public void Should_Have_Error_When_StartDate_After_DueDate()
        {
            var model = CreateValidRent();
            model.StartDate = DateTime.Now.AddDays(10);
            model.DueDate = DateTime.Now.AddDays(5);

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.StartDate);
        }

        [TestMethod]
        public void Should_Have_Error_When_ExtensionDaysTotal_Negative()
        {
            var model = CreateValidRent();
            model.ExtensionDaysTotal = -1;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.ExtensionDaysTotal);
        }

        [TestMethod]
        public void Should_Have_Error_When_NumberOfExtensions_Negative()
        {
            var model = CreateValidRent();
            model.NumberOfExtensions = -1;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(r => r.NumberOfExtensions);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Valid()
        {
            var model = CreateValidRent();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ReturnDate_Set()
        {
            var model = CreateValidRent();
            model.ReturnDate = DateTime.Now.AddDays(10);

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(r => r.ReturnDate);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ProcessedByEmployeeId_Set()
        {
            var model = CreateValidRent();
            model.ProcessedByEmployeeId = 99;

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(r => r.ProcessedByEmployeeId);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_ProcessedByEmployee_Set()
        {
            var model = CreateValidRent();
            model.ProcessedByEmployee = new Employee { Id = 99, Name = "John" };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(r => r.ProcessedByEmployee);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Reader_Set()
        {
            var model = CreateValidRent();
            model.Reader = new Reader { Id = model.ReaderId, Name = "Alice" };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(r => r.Reader);
        }

    }
}
