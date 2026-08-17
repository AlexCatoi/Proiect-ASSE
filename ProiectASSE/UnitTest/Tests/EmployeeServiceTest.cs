using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.Services.EmployeeService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _repoMock;
        private Mock<IValidator<Employee>> _validatorMock;
        private EmployeeService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _validatorMock = new Mock<IValidator<Employee>>();

            _service = new EmployeeService(_repoMock.Object, _validatorMock.Object);
        }

        // ---------------------------
        // REGISTER EMPLOYEE TESTS
        // ---------------------------

        [TestMethod]
        public void RegisterEmployee_Should_Add_When_Valid()
        {
            var emp = new Employee
            {
                Id = 1,
                Name = "John",
                Address = "Street",
                Email = "john@test.com",
                EnrollDate = DateTime.Now,
                EmployDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(emp)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.EmailExists(emp.Email, null)).Returns(false);
            _repoMock.Setup(r => r.PhoneExists(emp.Phone, null)).Returns(false);

            _service.RegisterEmployee(emp);

            _repoMock.Verify(r => r.Add(emp), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void RegisterEmployee_Should_Throw_When_Validation_Fails()
        {
            var emp = new Employee { Name = "" };

            var result = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            });

            _validatorMock.Setup(v => v.Validate(emp)).Returns(result);

            _service.RegisterEmployee(emp);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void RegisterEmployee_Should_Throw_When_Email_Exists()
        {
            var emp = new Employee
            {
                Name = "John",
                Address = "Test",
                Email = "john@test.com",
                EnrollDate = DateTime.Now,
                EmployDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(emp)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.EmailExists(emp.Email, null)).Returns(true);

            _service.RegisterEmployee(emp);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void RegisterEmployee_Should_Throw_When_Phone_Exists()
        {
            var emp = new Employee
            {
                Name = "John",
                Address = "Test",
                Phone = "12345",
                EnrollDate = DateTime.Now,
                EmployDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(emp)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.PhoneExists(emp.Phone, null)).Returns(true);

            _service.RegisterEmployee(emp);
        }

        // ---------------------------
        // UPDATE EMPLOYEE TESTS
        // ---------------------------

        [TestMethod]
        public void UpdateEmployee_Should_Update_When_Valid()
        {
            var emp = new Employee
            {
                Id = 1,
                Name = "Ana",
                Address = "Strada",
                Email = "ana@test.com",
                Phone = "123456",
                EnrollDate = DateTime.Now,
                EmployDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(emp)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.EmailExists(emp.Email, emp.Id)).Returns(false);
            _repoMock.Setup(r => r.PhoneExists(emp.Phone, emp.Id)).Returns(false);

            _service.UpdateEmployee(emp);

            _repoMock.Verify(r => r.Update(emp), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateEmployee_Should_Throw_When_Validation_Fails()
        {
            var emp = new Employee { Id = 1, Name = "" };

            var result = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            });

            _validatorMock.Setup(v => v.Validate(emp)).Returns(result);

            _service.UpdateEmployee(emp);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateEmployee_Should_Throw_When_Email_Exists()
        {
            var emp = new Employee
            {
                Id = 1,
                Name = "Ana",
                Address = "Strada",
                Email = "ana@test.com",
                EnrollDate = DateTime.Now,
                EmployDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(emp)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.EmailExists(emp.Email, emp.Id)).Returns(true);

            _service.UpdateEmployee(emp);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateEmployee_Should_Throw_When_Phone_Exists()
        {
            var emp = new Employee
            {
                Id = 1,
                Name = "Ana",
                Address = "Strada",
                Phone = "123456",
                EnrollDate = DateTime.Now,
                EmployDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(emp)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.PhoneExists(emp.Phone, emp.Id)).Returns(true);

            _service.UpdateEmployee(emp);
        }

        // ---------------------------
        // GET EMPLOYEE TESTS
        // ---------------------------

        [TestMethod]
        public void GetEmployee_Should_Return_Employee()
        {
            var emp = new Employee { Id = 1, Name = "Test" };

            _repoMock.Setup(r => r.GetById(1)).Returns(emp);

            var result = _service.GetEmployee(1);

            Assert.AreEqual(emp, result);
        }

        [TestMethod]
        public void GetEmployee_Should_Return_Null_When_Not_Found()
        {
            _repoMock.Setup(r => r.GetById(1)).Returns((Employee)null);

            var result = _service.GetEmployee(1);

            Assert.IsNull(result);
        }

        // ---------------------------
        // GET ALL EMPLOYEES TESTS
        // ---------------------------

        [TestMethod]
        public void GetAllEmployees_Should_Return_All()
        {
            var list = new List<Employee>
            {
                new Employee { Id = 1, Name = "A" },
                new Employee { Id = 2, Name = "B" }
            };

            _repoMock.Setup(r => r.GetAll()).Returns(list);

            var result = _service.GetAllEmployees();

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }

        // ---------------------------
        // DELETE EMPLOYEE TESTS
        // ---------------------------

        [TestMethod]
        public void DeleteEmployee_Should_Call_Delete_And_SaveChanges()
        {
            _service.DeleteEmployee(5);

            _repoMock.Verify(r => r.Delete(5), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void DeleteEmployee_Should_Not_Throw_When_Id_Not_Found()
        {
            _repoMock.Setup(r => r.Delete(99));

            _service.DeleteEmployee(99);

            _repoMock.Verify(r => r.Delete(99), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}
