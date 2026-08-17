using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.AuthorService;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AuthorServiceTests
    {
        private Mock<IAuthorRepository> _repo;
        private Mock<IValidator<Author>> _validator;
        private AuthorService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IAuthorRepository>();
            _validator = new Mock<IValidator<Author>>();
            _service = new AuthorService(_repo.Object, _validator.Object);
        }

        private Author CreateValidAuthor()
        {
            return new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Books = new List<Book>()
            };
        }

        [TestMethod]
        public void AddAuthor_Should_Add_When_Valid()
        {
            var author = CreateValidAuthor();
            _validator.Setup(v => v.Validate(author)).Returns(new ValidationResult());

            _service.AddAuthor(author);

            _repo.Verify(r => r.Add(author), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddAuthor_Should_Throw_When_Invalid()
        {
            var author = CreateValidAuthor();

            _validator.Setup(v => v.Validate(author))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("FirstName", "Error")
                }));

            _service.AddAuthor(author);
        }

        [TestMethod]
        public void UpdateAuthor_Should_Update_When_Valid()
        {
            var author = CreateValidAuthor();
            _validator.Setup(v => v.Validate(author)).Returns(new ValidationResult());

            _service.UpdateAuthor(author);

            _repo.Verify(r => r.Update(author), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateAuthor_Should_Throw_When_Invalid()
        {
            var author = CreateValidAuthor();

            _validator.Setup(v => v.Validate(author))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("LastName", "Error")
                }));

            _service.UpdateAuthor(author);
        }

        [TestMethod]
        public void DeleteAuthor_Should_Call_Repo()
        {
            _service.DeleteAuthor(5);

            _repo.Verify(r => r.Delete(5), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetAuthor_Should_Return_Author()
        {
            var author = CreateValidAuthor();
            _repo.Setup(r => r.GetById(1)).Returns(author);

            var result = _service.GetAuthor(1);

            Assert.AreEqual(author, result);
        }

        [TestMethod]
        public void GetAuthor_Should_Return_Null_When_Not_Found()
        {
            _repo.Setup(r => r.GetById(1)).Returns((Author)null);

            var result = _service.GetAuthor(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllAuthors_Should_Return_List()
        {
            var list = new List<Author> { CreateValidAuthor() };
            _repo.Setup(r => r.GetAll()).Returns(list);

            var result = _service.GetAllAuthors();

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }
    }
}
