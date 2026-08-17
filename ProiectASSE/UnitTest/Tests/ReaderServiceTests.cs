using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.Services.ReaderService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ReaderServiceTests
    {
        private Mock<IReaderRepository> _repoMock;
        private Mock<IValidator<Reader>> _validatorMock;
        private ReaderService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IReaderRepository>();
            _validatorMock = new Mock<IValidator<Reader>>();

            _service = new ReaderService(_repoMock.Object, _validatorMock.Object);
        }

        [TestMethod]
        public void RegisterReader_Should_Add_Reader_When_Valid()
        {
            var reader = new Reader
            {
                Id = 1,
                Name = "John Doe",
                Address = "Street 1",
                Email = "john@test.com",
                EnrollDate = DateTime.Now
            };

            _validatorMock
                .Setup(v => v.Validate(reader))
                .Returns(new ValidationResult());

            _repoMock.Setup(r => r.EmailExists(reader.Email, null)).Returns(false);
            _repoMock.Setup(r => r.PhoneExists(reader.Phone, null)).Returns(false);

            _service.RegisterReader(reader);

            _repoMock.Verify(r => r.Add(reader), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void RegisterReader_Should_Throw_When_Validation_Fails()
        {
            var reader = new Reader { Name = "" };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            });

            _validatorMock
                .Setup(v => v.Validate(reader))
                .Returns(validationResult);

            _service.RegisterReader(reader);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void RegisterReader_Should_Throw_When_Email_Exists()
        {
            var reader = new Reader
            {
                Name = "John",
                Address = "Test",
                Email = "john@test.com",
                EnrollDate = DateTime.Now
            };

            _validatorMock
                .Setup(v => v.Validate(reader))
                .Returns(new ValidationResult());

            _repoMock.Setup(r => r.EmailExists(reader.Email, null)).Returns(true);

            _service.RegisterReader(reader);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void RegisterReader_Should_Throw_When_Phone_Exists()
        {
            // Arrange
            var reader = new Reader
            {
                Name = "John",
                Address = "Test",
                Phone = "12345",
                EnrollDate = DateTime.Now
            };

            _validatorMock
                .Setup(v => v.Validate(reader))
                .Returns(new ValidationResult());

            _repoMock.Setup(r => r.PhoneExists(reader.Phone, null)).Returns(true);

            _service.RegisterReader(reader);
        }

        [TestMethod]
        public void UpdateReader_Should_Update_When_Valid()
        {
            var reader = new Reader
            {
                Id = 1,
                Name = "Ana",
                Address = "Strada",
                Email = "ana@test.com",
                Phone = "123456",
                EnrollDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(reader)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.EmailExists(reader.Email, reader.Id)).Returns(false);
            _repoMock.Setup(r => r.PhoneExists(reader.Phone, reader.Id)).Returns(false);

            _service.UpdateReader(reader);

            _repoMock.Verify(r => r.Update(reader), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateReader_Should_Throw_When_Validation_Fails()
        {
            var reader = new Reader { Id = 1, Name = "" };

            var result = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            });

            _validatorMock.Setup(v => v.Validate(reader)).Returns(result);

            _service.UpdateReader(reader);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateReader_Should_Throw_When_Email_Exists()
        {
            var reader = new Reader
            {
                Id = 1,
                Name = "Ana",
                Address = "Strada",
                Email = "ana@test.com",
                EnrollDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(reader)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.EmailExists(reader.Email, reader.Id)).Returns(true);

            _service.UpdateReader(reader);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateReader_Should_Throw_When_Phone_Exists()
        {
            var reader = new Reader
            {
                Id = 1,
                Name = "Ana",
                Address = "Strada",
                Phone = "123456",
                EnrollDate = DateTime.Now
            };

            _validatorMock.Setup(v => v.Validate(reader)).Returns(new ValidationResult());
            _repoMock.Setup(r => r.PhoneExists(reader.Phone, reader.Id)).Returns(true);

            _service.UpdateReader(reader);
        }

        [TestMethod]
        public void GetReader_Should_Return_Reader()
        {
            var reader = new Reader { Id = 1, Name = "Test" };

            _repoMock.Setup(r => r.GetById(1)).Returns(reader);

            var result = _service.GetReader(1);

            Assert.AreEqual(reader, result);
        }

        [TestMethod]
        public void GetReader_Should_Return_Null_When_Not_Found()
        {
            _repoMock.Setup(r => r.GetById(1)).Returns((Reader)null);

            var result = _service.GetReader(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllReaders_Should_Return_All_Readers()
        {
            var list = new List<Reader>
    {
        new Reader { Id = 1, Name = "A" },
        new Reader { Id = 2, Name = "B" }
    };

            _repoMock.Setup(r => r.GetAll()).Returns(list);

            var result = _service.GetAllReaders();

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }

        [TestMethod]
        public void DeleteReader_Should_Call_Delete_And_SaveChanges()
        {
            _service.DeleteReader(5);

            _repoMock.Verify(r => r.Delete(5), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void DeleteReader_Should_Not_Throw_When_Id_Not_Found()
        {

            _repoMock.Setup(r => r.Delete(99));

            _service.DeleteReader(99);

            _repoMock.Verify(r => r.Delete(99), Times.Once);
            _repoMock.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}
