using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.RentService;
using System;
using System.Collections.Generic;
using System.Linq;
using ProiectASSE.Services.BookCopyService;
using ProiectASSE.Services.ReaderService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RentServiceTests
    {
        private Mock<IRentRepository> _repo;
        private Mock<IValidator<Rent>> _validator;
        private Mock<IRentRulesService> _rules;
        private Mock<IBookCopyService> _copyService;
        private Mock<IReaderService> _readerService;

        private RentService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IRentRepository>();
            _validator = new Mock<IValidator<Rent>>();
            _rules = new Mock<IRentRulesService>();
            _copyService = new Mock<IBookCopyService>();
            _readerService = new Mock<IReaderService>();

            _service = new RentService(
                _repo.Object,
                _validator.Object,
                _rules.Object,
                _copyService.Object,
                _readerService.Object
            );
        }

        private BookCopy CreateCopy(int id)
        {
            return new BookCopy
            {
                Id = id,
                BookId = id,
                Book = new Book
                {
                    Id = id,
                    Categories = new List<Category>
                    {
                        new Category { Name = "A" }
                    }
                }
            };
        }

        [TestMethod]
        public void CreateRent_Should_Create_When_Valid()
        {
            var reader = new Reader { Id = 1};
            _readerService.Setup(r => r.GetReader(1)).Returns(reader);

            var copy = CreateCopy(10);
            _copyService.Setup(c => c.GetCopy(10)).Returns(copy);

            _validator.Setup(v => v.Validate(It.IsAny<Rent>()))
                .Returns(new ValidationResult());

            var result = _service.CreateRent(1, new List<int> { 10 });

            _repo.Verify(r => r.Add(It.IsAny<Rent>()), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateRent_Should_Throw_When_Reader_Not_Found()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns((Reader)null);

            _service.CreateRent(1, new List<int> { 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateRent_Should_Throw_When_Copy_Not_Found()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns(new Reader());

            _copyService.Setup(c => c.GetCopy(10)).Returns((BookCopy)null);

            _service.CreateRent(1, new List<int> { 10 });
        }

        [TestMethod]
        public void ReturnRent_Should_Update_Status_And_Copies()
        {
            var rent = new Rent
            {
                Id = 1,
                Status = RentStatus.ACTIVE,
                BookCopies = new List<BookCopy> { CreateCopy(10) }
            };

            _repo.Setup(r => r.GetById(1)).Returns(rent);

            _service.ReturnRent(1);

            Assert.AreEqual(RentStatus.RETURNED, rent.Status);
            Assert.IsFalse(rent.BookCopies.First().IsBorrowed);

            _repo.Verify(r => r.Update(rent), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void ExtendRent_Should_Update_DueDate_And_Extensions()
        {
            var rent = new Rent
            {
                Id = 1,
                DueDate = DateTime.Today,
                NumberOfExtensions = 0,
                ExtensionDaysTotal = 0
            };

            _repo.Setup(r => r.GetById(1)).Returns(rent);

            _service.ExtendRent(1, 5);

            Assert.AreEqual(DateTime.Today.AddDays(5), rent.DueDate);
            Assert.AreEqual(1, rent.NumberOfExtensions);
            Assert.AreEqual(5, rent.ExtensionDaysTotal);

            _repo.Verify(r => r.Update(rent), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateRent_Should_Throw_When_Copy_IsReadingRoomOnly()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns(new Reader());
            var copy = new BookCopy { Id = 10, IsReadingRoomOnly = true };
            _copyService.Setup(c => c.GetCopy(10)).Returns(copy);

            _service.CreateRent(1, new List<int> { 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateRent_Should_Throw_When_Copy_IsBorrowed()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns(new Reader());
            var copy = new BookCopy { Id = 10, IsBorrowed = true };
            _copyService.Setup(c => c.GetCopy(10)).Returns(copy);

            _service.CreateRent(1, new List<int> { 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateRent_Should_Throw_When_Rules_Fail()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns(new Reader());
            var copy = CreateCopy(10);
            _copyService.Setup(c => c.GetCopy(10)).Returns(copy);

            _rules.Setup(r => r.ValidateRentRequest(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<List<BookCopy>>(), It.IsAny<List<string>>()))
                  .Throws(new Exception("Rule failed"));

            _validator.Setup(v => v.Validate(It.IsAny<Rent>()))
                     .Returns(new ValidationResult());

            _service.CreateRent(1, new List<int> { 10 });
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void CreateRent_Should_Throw_When_Validator_Fails()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns(new Reader());
            var copy = CreateCopy(10);
            _copyService.Setup(c => c.GetCopy(10)).Returns(copy);

            _validator.Setup(v => v.Validate(It.IsAny<Rent>()))
                     .Returns(new ValidationResult(new List<ValidationFailure>
                     {
                 new ValidationFailure("ReaderId", "Invalid")
                     }));

            _service.CreateRent(1, new List<int> { 10 });
        }

        [TestMethod]
        public void CreateRent_Should_Handle_Employee_Reader()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns(new Employee());
            var copy = CreateCopy(10);
            _copyService.Setup(c => c.GetCopy(10)).Returns(copy);

            _validator.Setup(v => v.Validate(It.IsAny<Rent>()))
                     .Returns(new ValidationResult());

            _service.CreateRent(1, new List<int> { 10 });

            _repo.Verify(r => r.Add(It.IsAny<Rent>()), Times.Once);
        }

        [TestMethod]
        public void CreateRent_Should_Set_IsBorrowed_For_All_Copies()
        {
            _readerService.Setup(r => r.GetReader(1)).Returns(new Reader());

            var copy1 = CreateCopy(10);
            var copy2 = CreateCopy(11);

            _copyService.Setup(c => c.GetCopy(10)).Returns(copy1);
            _copyService.Setup(c => c.GetCopy(11)).Returns(copy2);

            _validator.Setup(v => v.Validate(It.IsAny<Rent>()))
                     .Returns(new ValidationResult());

            _service.CreateRent(1, new List<int> { 10, 11 });

            Assert.IsTrue(copy1.IsBorrowed);
            Assert.IsTrue(copy2.IsBorrowed);
        }

        [TestMethod]
        public void GetActiveRents_Should_Return_Rents_From_Repo()
        {
            // Arrange
            var rents = new List<Rent>
            {
                new Rent { Id = 1 },
                new Rent { Id = 2 }
            };

            _repo.Setup(r => r.GetActiveRentsForReader(10)).Returns(rents);

            var result = _service.GetActiveRents(10).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);

            _repo.Verify(r => r.GetActiveRentsForReader(10), Times.Once);
        }


    }
}
