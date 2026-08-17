using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.Services.CategoryService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _repo;
        private Mock<IValidator<Category>> _validator;
        private CategoryService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<ICategoryRepository>();
            _validator = new Mock<IValidator<Category>>();
            _service = new CategoryService(_repo.Object, _validator.Object);
        }

        [TestMethod]
        public void AddCategory_Should_Add_When_Valid()
        {
            var cat = new Category { Id = 1, Name = "Science" };

            _validator.Setup(v => v.Validate(cat)).Returns(new ValidationResult());

            _service.AddCategory(cat);

            _repo.Verify(r => r.Add(cat), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddCategory_Should_Throw_When_Invalid()
        {
            var cat = new Category { Name = "" };

            _validator.Setup(v => v.Validate(cat))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Name", "Required")
                }));

            _service.AddCategory(cat);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddCategory_Should_Throw_When_Creating_Cycle()
        {
            var cat = new Category { Id = 1, ParentId = 2 };

            _validator.Setup(v => v.Validate(cat)).Returns(new ValidationResult());

            _repo.Setup(r => r.GetById(2)).Returns(new Category { Id = 2, ParentId = 1 });

            _service.AddCategory(cat);
        }

        [TestMethod]
        public void IsAncestor_Should_Return_True()
        {
            _repo.Setup(r => r.GetById(3)).Returns(new Category { Id = 3, ParentId = 2 });
            _repo.Setup(r => r.GetById(2)).Returns(new Category { Id = 2, ParentId = 1 });
            _repo.Setup(r => r.GetById(1)).Returns(new Category { Id = 1 });

            Assert.IsTrue(_service.IsAncestor(1, 3));
        }

        [TestMethod]
        public void IsAncestor_Should_Return_False()
        {
            _repo.Setup(r => r.GetById(3)).Returns(new Category { Id = 3, ParentId = null });

            Assert.IsFalse(_service.IsAncestor(1, 3));
        }

        [TestMethod]
        public void GetAncestors_Should_Return_List()
        {
            _repo.Setup(r => r.GetById(3)).Returns(new Category { Id = 3, ParentId = 2 });
            _repo.Setup(r => r.GetById(2)).Returns(new Category { Id = 2, ParentId = 1 });
            _repo.Setup(r => r.GetById(1)).Returns(new Category { Id = 1 });

            var result = _service.GetAncestors(3);

            Assert.AreEqual(2, ((List<Category>)result).Count);
        }

        [TestMethod]
        public void GetDescendants_Should_Return_List()
        {
            var all = new List<Category>
            {
                new Category { Id = 1 },
                new Category { Id = 2, ParentId = 1 },
                new Category { Id = 3, ParentId = 2 }
            };

            _repo.Setup(r => r.GetAll()).Returns(all);

            var result = _service.GetDescendants(1);

            Assert.AreEqual(2, ((List<Category>)result).Count);
        }

        [TestMethod]
        public void UpdateCategory_Should_Update_When_Valid()
        {
            var cat = new Category { Id = 1, Name = "Science" };

            _validator.Setup(v => v.Validate(cat)).Returns(new ValidationResult());

            _service.UpdateCategory(cat);

            _repo.Verify(r => r.Update(cat), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateCategory_Should_Throw_When_Invalid()
        {
            var cat = new Category { Id = 1, Name = "" };

            _validator.Setup(v => v.Validate(cat))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
            new ValidationFailure("Name", "Required")
                }));

            _service.UpdateCategory(cat);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void UpdateCategory_Should_Throw_When_Creating_Cycle()
        {
            var cat = new Category { Id = 1, ParentId = 2 };

            _validator.Setup(v => v.Validate(cat)).Returns(new ValidationResult());

            _repo.Setup(r => r.GetById(2)).Returns(new Category { Id = 2, ParentId = 1 });

            _service.UpdateCategory(cat);
        }


        [TestMethod]
        public void DeleteCategory_Should_Call_Delete_And_SaveChanges()
        {
            _service.DeleteCategory(5);

            _repo.Verify(r => r.Delete(5), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void DeleteCategory_Should_Not_Throw_When_Id_Not_Found()
        {
            _repo.Setup(r => r.Delete(99));

            _service.DeleteCategory(99);

            _repo.Verify(r => r.Delete(99), Times.Once);
            _repo.Verify(r => r.SaveChanges(), Times.Once);
        }


        [TestMethod]
        public void GetCategory_Should_Return_Category()
        {
            var cat = new Category { Id = 1, Name = "Test" };

            _repo.Setup(r => r.GetById(1)).Returns(cat);

            var result = _service.GetCategory(1);

            Assert.AreEqual(cat, result);
        }

        [TestMethod]
        public void GetCategory_Should_Return_Null_When_Not_Found()
        {
            _repo.Setup(r => r.GetById(1)).Returns((Category)null);

            var result = _service.GetCategory(1);

            Assert.IsNull(result);
        }


        [TestMethod]
        public void GetAllCategories_Should_Return_List()
        {
            var list = new List<Category>
    {
        new Category { Id = 1 },
        new Category { Id = 2 }
    };

            _repo.Setup(r => r.GetAll()).Returns(list);

            var result = _service.GetAllCategories();

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateCategoryTree_Should_Throw_When_Validator_Fails()
        {
            var cat = new Category { Name = "" };

            _validator.Setup(v => v.Validate(cat))
                .Returns(new ValidationResult(new List<ValidationFailure>
                {
            new ValidationFailure("Name", "Required")
                }));

            _service.ValidateCategoryTree(cat);
        }

        [TestMethod]
        public void ValidateCategoryTree_Should_Not_Throw_When_ParentId_Is_Null()
        {
            var cat = new Category { Id = 1, Name = "Science", ParentId = null };

            _validator.Setup(v => v.Validate(cat)).Returns(new ValidationResult());

            _service.ValidateCategoryTree(cat);
        }

        [TestMethod]
        public void ValidateCategoryTree_Should_Not_Throw_When_No_Cycle()
        {
            var cat = new Category { Id = 1, ParentId = 2 };

            _validator.Setup(v => v.Validate(cat)).Returns(new ValidationResult());

            _repo.Setup(r => r.GetById(2)).Returns(new Category { Id = 2, ParentId = null });

            _service.ValidateCategoryTree(cat);
        }

        [TestMethod]
        public void IsDescendant_Should_Return_True()
        {
            _repo.Setup(r => r.GetById(3)).Returns(new Category { Id = 3, ParentId = 2 });
            _repo.Setup(r => r.GetById(2)).Returns(new Category { Id = 2, ParentId = 1 });
            _repo.Setup(r => r.GetById(1)).Returns(new Category { Id = 1 });

            Assert.IsTrue(_service.IsDescendant(3, 1));
        }

        [TestMethod]
        public void IsDescendant_Should_Return_False()
        {
            _repo.Setup(r => r.GetById(3)).Returns(new Category { Id = 3, ParentId = null });

            Assert.IsFalse(_service.IsDescendant(3, 1));
        }

    }

}
