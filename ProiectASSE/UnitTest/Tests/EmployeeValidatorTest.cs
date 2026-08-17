using System;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.EmployeeService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EmployeeValidatorTests
    {
        private EmployeeValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new EmployeeValidator();
        }

        [TestMethod]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new Employee { Name = "" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.Name);
        }

        [TestMethod]
        public void Should_Have_Error_When_Address_Is_Empty()
        {
            var model = new Employee { Address = "" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.Address);
        }

        [TestMethod]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new Employee { Email = "invalid-email" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.Email);
        }

        [TestMethod]
        public void Should_Have_Error_When_Phone_Is_Invalid()
        {
            var model = new Employee { Phone = "abc123" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.Phone);
        }

        [TestMethod]
        public void Should_Have_Error_When_No_Email_And_No_Phone()
        {
            var model = new Employee
            {
                Name = "John",
                Address = "Street",
                Email = "",
                Phone = ""
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e);
        }

        [TestMethod]
        public void Should_Have_Error_When_EnrollDate_In_Future()
        {
            var model = new Employee
            {
                EnrollDate = DateTime.Now.AddSeconds(1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.EnrollDate);
        }

        [TestMethod]
        public void Should_Have_Error_When_EmployDate_Before_EnrollDate()
        {
            var model = new Employee
            {
                EnrollDate = DateTime.Today,
                EmployDate = DateTime.Today.AddDays(-1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(e => e.EmployDate);
        }

        [TestMethod]
        public void Should_Not_Have_Error_When_Employee_Is_Valid()
        {
            var model = new Employee
            {
                Name = "John",
                Address = "Street",
                Email = "john@test.com",
                Phone = "12345",
                EnrollDate = DateTime.Today,
                EmployDate = DateTime.Today.AddDays(1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
