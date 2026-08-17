using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.BookCopyService;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BookCopyServiceTests
    {
        private Mock<IBookCopyRepository> _repo;
        private Mock<IValidator<BookCopy>> _validator;
        private BookCopyService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IBookCopyRepository>();
            _validator = new Mock<IValidator<BookCopy>>();
            _service = new BookCopyService(_repo.Object, _validator.Object);
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

        // ADD
        [TestMethod]
        public void AddCopy_Should_Add_When_Valid()
        {
            var copy = CreateValidCopy();
            _validator.Setup(v => v.Validate(copy)).Returns(new ValidationResult());

            _service.AddCopy(copy);

            _repo.Verify(r => r.Add(copy), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddCopy_Should_Throw_When_Invalid()
        {
            var copy = CreateValidCopy();

            _validator.Setup(v => v.Validate(copy))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("BookId", "Error")
                }));

            _service.AddCopy(copy);
        }

        // UPDATE
        [TestMethod]
        public void UpdateCopy_Should_Update_When_Valid()
        {
            var copy = CreateValidCopy();
            _validator.Setup(v => v.Validate(copy)).Returns(new ValidationResult());

            _service.UpdateCopy(copy);

            _repo.Verify(r => r.Update(copy), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateCopy_Should_Throw_When_Invalid()
        {
            var copy = CreateValidCopy();

            _validator.Setup(v => v.Validate(copy))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("BookId", "Error")
                }));

            _service.UpdateCopy(copy);
        }

        // DELETE
        [TestMethod]
        public void DeleteCopy_Should_Call_Repo()
        {
            _service.DeleteCopy(5);

            _repo.Verify(r => r.Delete(5), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        // GET
        [TestMethod]
        public void GetCopy_Should_Return_Copy()
        {
            var copy = CreateValidCopy();
            _repo.Setup(r => r.GetById(1)).Returns(copy);

            var result = _service.GetCopy(1);

            Assert.AreEqual(copy, result);
        }

        [TestMethod]
        public void GetCopy_Should_Return_Null_When_Not_Found()
        {
            _repo.Setup(r => r.GetById(1)).Returns((BookCopy)null);

            var result = _service.GetCopy(1);

            Assert.IsNull(result);
        }

        // GET ALL
        [TestMethod]
        public void GetAllCopies_Should_Return_List()
        {
            var list = new List<BookCopy> { CreateValidCopy() };
            _repo.Setup(r => r.GetAll()).Returns(list);

            var result = _service.GetAllCopies();

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }

        // CAN BE BORROWED
        [TestMethod]
        public void CanBeBorrowed_Should_Return_False_When_ReadingRoomOnly()
        {
            var copy = CreateValidCopy();
            copy.IsReadingRoomOnly = true;

            _repo.Setup(r => r.GetById(1)).Returns(copy);

            Assert.IsFalse(_service.CanBeBorrowed(1));
        }

        [TestMethod]
        public void CanBeBorrowed_Should_Return_False_When_Borrowed()
        {
            var copy = CreateValidCopy();
            copy.IsBorrowed = true;

            _repo.Setup(r => r.GetById(1)).Returns(copy);

            Assert.IsFalse(_service.CanBeBorrowed(1));
        }

        [TestMethod]
        public void CanBeBorrowed_Should_Return_False_When_ActiveRent()
        {
            var copy = CreateValidCopy();
            copy.Rents.Add(new Rent { Status = RentStatus.ACTIVE });

            _repo.Setup(r => r.GetById(1)).Returns(copy);

            Assert.IsFalse(_service.CanBeBorrowed(1));
        }

        [TestMethod]
        public void CanBeBorrowed_Should_Return_True_When_Valid()
        {
            var copy = CreateValidCopy();
            _repo.Setup(r => r.GetById(1)).Returns(copy);

            Assert.IsTrue(_service.CanBeBorrowed(1));
        }
    }
}
