using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.BookService;
using ProiectASSE.Services.CategoryService;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BookServiceTests
    {
        private Mock<IBookRepository> _repo;
        private Mock<ICategoryService> _categoryService;
        private Mock<IValidator<Book>> _validator;
        private BookService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IBookRepository>();
            _categoryService = new Mock<ICategoryService>();
            _validator = new Mock<IValidator<Book>>();

            _service = new BookService(_repo.Object, _categoryService.Object, _validator.Object);
        }

        private Book CreateValidBook()
        {
            return new Book
            {
                Title = "Test",
                Description = "Desc",
                Categories = new List<Category> { new Category { Id = 1, Name = "A" } },
                Authors = new List<Author> { new Author() },
                Editions = new List<Edition> { new Edition() },
                Copies = new List<BookCopy>
                {
                    new BookCopy { IsReadingRoomOnly = false, IsBorrowed = false }
                }
            };
        }


        [TestMethod]
        public void AddBook_Should_Add_When_Valid()
        {
            var book = CreateValidBook();

            _validator.Setup(v => v.Validate(book)).Returns(new ValidationResult());

            _service.AddBook(book);

            _repo.Verify(r => r.Add(book), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Validator_Fails()
        {
            var book = CreateValidBook();

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Title", "Error")
                }));

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Categories_Invalid()
        {
            var book = CreateValidBook();
            book.Categories.Add(new Category { Id = 2, Name = "B" });

            _validator.Setup(v => v.Validate(book)).Returns(new ValidationResult());

            _categoryService.Setup(c => c.IsAncestor(1, 2)).Returns(true);

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Copies_Invalid()
        {
            var book = CreateValidBook();
            book.Copies = new List<BookCopy>
            {
                new BookCopy { IsReadingRoomOnly = true }
            };

            _validator.Setup(v => v.Validate(book)).Returns(new ValidationResult());

            _service.AddBook(book);
        }


        [TestMethod]
        public void UpdateBook_Should_Update_When_Valid()
        {
            var book = CreateValidBook();

            _validator.Setup(v => v.Validate(book)).Returns(new ValidationResult());

            _service.UpdateBook(book);

            _repo.Verify(r => r.Update(book), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateBook_Should_Throw_When_Validator_Fails()
        {
            var book = CreateValidBook();

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Title", "Error")
                }));

            _service.UpdateBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateBook_Should_Throw_When_Categories_Invalid()
        {
            var book = CreateValidBook();
            book.Categories.Add(new Category { Id = 2 });

            _validator.Setup(v => v.Validate(book)).Returns(new ValidationResult());

            _categoryService.Setup(c => c.IsAncestor(1, 2)).Returns(true);

            _service.UpdateBook(book);
        }


        [TestMethod]
        public void DeleteBook_Should_Call_Repo()
        {
            _service.DeleteBook(5);

            _repo.Verify(r => r.Delete(5), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }


        [TestMethod]
        public void GetBook_Should_Return_Book()
        {
            var book = CreateValidBook();
            _repo.Setup(r => r.GetById(1)).Returns(book);

            var result = _service.GetBook(1);

            Assert.AreEqual(book, result);
        }

        [TestMethod]
        public void GetBook_Should_Return_Null_When_Not_Found()
        {
            _repo.Setup(r => r.GetById(1)).Returns((Book)null);

            var result = _service.GetBook(1);

            Assert.IsNull(result);
        }


        [TestMethod]
        public void GetAllBooks_Should_Return_List()
        {
            var list = new List<Book> { CreateValidBook() };

            _repo.Setup(r => r.GetAll()).Returns(list);

            var result = _service.GetAllBooks();

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }


        [TestMethod]
        public void CanBeBorrowed_Should_Return_False_When_All_ReadingRoom()
        {
            var book = CreateValidBook();
            book.Copies = new List<BookCopy>
            {
                new BookCopy { IsReadingRoomOnly = true }
            };

            _repo.Setup(r => r.GetById(1)).Returns(book);

            Assert.IsFalse(_service.CanBeBorrowed(1));
        }

        [TestMethod]
        public void CanBeBorrowed_Should_Return_False_When_Less_Than_10_Percent()
        {
            var book = CreateValidBook();
            book.Copies = new List<BookCopy>
    {
        new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
        new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
        new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
        new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
        new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
        new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true }
    };

            _repo.Setup(r => r.GetById(1)).Returns(book);

            Assert.IsFalse(_service.CanBeBorrowed(1));
        }


        [TestMethod]
        public void CanBeBorrowed_Should_Return_True_When_Valid()
        {
            var book = CreateValidBook();
            _repo.Setup(r => r.GetById(1)).Returns(book);

            Assert.IsTrue(_service.CanBeBorrowed(1));
        }


        [TestMethod]
        public void GetAllCategoriesForBook_Should_Return_Categories_And_Ancestors()
        {
            var book = CreateValidBook();
            book.Categories = new List<Category>
    {
        new Category { Id = 1, Name = "A" }
    };

            _repo.Setup(r => r.GetById(1)).Returns(book);

            _categoryService.Setup(c => c.GetAncestors(1))
                .Returns(new List<Category> { new Category { Id = 99, Name = "Ancestor" } });

            var result = _service.GetAllCategoriesForBook(1);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.IsTrue(resultList.Any(c => c.Id == 1));
            Assert.IsTrue(resultList.Any(c => c.Id == 99));
        }

        [TestMethod]
        public void GetAllCategoriesForBook_Should_Return_All_Categories_And_All_Ancestors_For_Multiple_Categories()
        {
            var book = CreateValidBook();
            book.Categories = new List<Category>
    {
        new Category { Id = 1, Name = "A" },
        new Category { Id = 2, Name = "B" }
    };

            _repo.Setup(r => r.GetById(1)).Returns(book);

            _categoryService.Setup(c => c.GetAncestors(1))
                .Returns(new List<Category> { new Category { Id = 10, Name = "AncestorA" } });

            _categoryService.Setup(c => c.GetAncestors(2))
                .Returns(new List<Category> { new Category { Id = 20, Name = "AncestorB" } });

            var result = _service.GetAllCategoriesForBook(1).ToList();

            Assert.AreEqual(4, result.Count); 
            Assert.IsTrue(result.Any(c => c.Id == 1));
            Assert.IsTrue(result.Any(c => c.Id == 2));
            Assert.IsTrue(result.Any(c => c.Id == 10));
            Assert.IsTrue(result.Any(c => c.Id == 20));
        }
        [TestMethod]
        public void GetAllCategoriesForBook_Should_Return_Only_Categories_When_No_Ancestors()
        {
            var book = CreateValidBook();
            book.Categories = new List<Category>
    {
        new Category { Id = 1, Name = "A" },
        new Category { Id = 2, Name = "B" }
    };

            _repo.Setup(r => r.GetById(1)).Returns(book);

            _categoryService.Setup(c => c.GetAncestors(It.IsAny<int>()))
                .Returns(new List<Category>()); 

            var result = _service.GetAllCategoriesForBook(1).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(c => c.Id == 1));
            Assert.IsTrue(result.Any(c => c.Id == 2));
        }
        [TestMethod]
        public void GetAllCategoriesForBook_Should_Return_Empty_When_Book_Not_Found()
        {
            _repo.Setup(r => r.GetById(1)).Returns((Book)null);


            var result = _service.GetAllCategoriesForBook(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Categories_Null()
        {
            var book = CreateValidBook();
            book.Categories = null;

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Categories_Empty()
        {
            var book = CreateValidBook();
            book.Categories = new List<Category>();

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Categories_Exceed_Max()
        {
            var book = CreateValidBook();
            book.Categories = new List<Category>
            {
                new Category { Id = 1, Name = "A" },
                new Category { Id = 2, Name = "B" },
                new Category { Id = 3, Name = "C" },
                new Category { Id = 4, Name = "D" },
                new Category { Id = 5, Name = "E" },
                new Category { Id = 6, Name = "F" },
            };

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Categories_Invalid_ReverseAncestor()
        {
            var book = CreateValidBook();
            book.Categories = new List<Category>
            {
                new Category { Id = 1, Name = "A" },
                new Category { Id = 2, Name = "B" }
            };

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _categoryService.Setup(c => c.IsAncestor(1, 2)).Returns(false);
            _categoryService.Setup(c => c.IsAncestor(2, 1)).Returns(true);

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Copies_Null()
        {
            var book = CreateValidBook();
            book.Copies = null;

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Copies_Empty()
        {
            var book = CreateValidBook();
            book.Copies = new List<BookCopy>();

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_All_Copies_ReadingRoomOnly()
        {
            var book = CreateValidBook();
            book.Copies = new List<BookCopy>
            {
                new BookCopy { IsReadingRoomOnly = true },
                new BookCopy { IsReadingRoomOnly = true }
            };

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _service.AddBook(book);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddBook_Should_Throw_When_Less_Than_10Percent_Available()
        {
            var book = CreateValidBook();
            book.Copies = new List<BookCopy>
            {
                new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
                new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
                new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
                new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
                new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true },
                new BookCopy { IsReadingRoomOnly = false, IsBorrowed = true }
            };

            _validator.Setup(v => v.Validate(book))
                .Returns(new ValidationResult());

            _service.AddBook(book);
        }

    }
}
