using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.EditionService;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class EditionServiceTests
    {
        private Mock<IEditionRepository> _repo;
        private Mock<IValidator<Edition>> _validator;
        private EditionService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IEditionRepository>();
            _validator = new Mock<IValidator<Edition>>();
            _service = new EditionService(_repo.Object, _validator.Object);
        }

        private Edition CreateValidEdition()
        {
            return new Edition
            {
                Id = 1,
                Publisher = "Test",
                Year = 2020,
                Pages = 100,
                BookType = "Hardcover",
                BookId = 10
            };
        }

        [TestMethod]
        public void AddEdition_Should_Add_When_Valid()
        {
            var edition = CreateValidEdition();
            _validator.Setup(v => v.Validate(edition)).Returns(new ValidationResult());

            _service.AddEdition(edition);

            _repo.Verify(r => r.Add(edition), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddEdition_Should_Throw_When_Invalid()
        {
            var edition = CreateValidEdition();

            _validator.Setup(v => v.Validate(edition))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Publisher", "Error")
                }));

            _service.AddEdition(edition);
        }

        [TestMethod]
        public void UpdateEdition_Should_Update_When_Valid()
        {
            var edition = CreateValidEdition();
            _validator.Setup(v => v.Validate(edition)).Returns(new ValidationResult());

            _service.UpdateEdition(edition);

            _repo.Verify(r => r.Update(edition), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateEdition_Should_Throw_When_Invalid()
        {
            var edition = CreateValidEdition();

            _validator.Setup(v => v.Validate(edition))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Publisher", "Error")
                }));

            _service.UpdateEdition(edition);
        }

        [TestMethod]
        public void DeleteEdition_Should_Call_Repo()
        {
            _service.DeleteEdition(5);

            _repo.Verify(r => r.Delete(5), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetEdition_Should_Return_Edition()
        {
            var edition = CreateValidEdition();
            _repo.Setup(r => r.GetById(1)).Returns(edition);

            var result = _service.GetEdition(1);

            Assert.AreEqual(edition, result);
        }

        [TestMethod]
        public void GetEdition_Should_Return_Null_When_Not_Found()
        {
            _repo.Setup(r => r.GetById(1)).Returns((Edition)null);

            var result = _service.GetEdition(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllEditions_Should_Return_List()
        {
            var list = new List<Edition> { CreateValidEdition() };
            _repo.Setup(r => r.GetAll()).Returns(list);

            var result = _service.GetAllEditions();

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }
    }
}
