using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProiectASSE.DataAccess.Repository;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.RentService;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RentHistoryServiceTests
    {
        private Mock<IRentRepository> _repo;
        private RentHistoryService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IRentRepository>();
            _service = new RentHistoryService(_repo.Object);
        }

        private Rent CreateRent()
        {
            return new Rent
            {
                Id = 1,
                ReaderId = 10,
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                Status = RentStatus.ACTIVE,
                BookCopies = new List<BookCopy>()
            };
        }

        [TestMethod]
        public void GetActiveRents_Should_Call_Repo()
        {
            var list = new List<Rent> { CreateRent() };
            _repo.Setup(r => r.GetActiveRentsForReader(10)).Returns(list);

            var result = _service.GetActiveRents(10);

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }

        [TestMethod]
        public void GetRentsInPeriod_Should_Call_Repo()
        {
            var list = new List<Rent> { CreateRent() };
            var start = DateTime.Now.AddMonths(-1);
            var end = DateTime.Now;

            _repo.Setup(r => r.GetRentsForReaderInPeriod(10, start, end)).Returns(list);

            var result = _service.GetRentsInPeriod(10, start, end);

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }

        [TestMethod]
        public void GetRentsForBook_Should_Call_Repo()
        {
            var list = new List<Rent> { CreateRent() };

            _repo.Setup(r => r.GetRentsForReaderAndBook(10, 5)).Returns(list);

            var result = _service.GetRentsForBook(10, 5);

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }

        [TestMethod]
        public void GetRentsForDay_Should_Call_Repo()
        {
            var list = new List<Rent> { CreateRent() };
            var date = DateTime.Today;

            _repo.Setup(r => r.GetRentsForReaderOnDate(10, date)).Returns(list);

            var result = _service.GetRentsForDay(10, date);

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }

        [TestMethod]
        public void GetExtensionsInLast3Months_Should_Call_Repo()
        {
            var list = new List<Rent> { CreateRent() };

            _repo.Setup(r => r.GetExtensionsForReaderInLast3Months(10)).Returns(list);

            var result = _service.GetExtensionsInLast3Months(10);

            CollectionAssert.AreEqual(list, (System.Collections.ICollection)result);
        }
    }
}
