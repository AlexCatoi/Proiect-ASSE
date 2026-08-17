using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProiectASSE.DomainModel.Entities;
using ProiectASSE.Services.RentService;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RentRulesServiceTests
    {
        private Mock<IRentHistoryService> _history;
        private Mock<IRentLimitService> _limits;
        private RentRulesService _service;

        [TestInitialize]
        public void Setup()
        {
            _history = new Mock<IRentHistoryService>();
            _limits = new Mock<IRentLimitService>();
            _service = new RentRulesService(_history.Object, _limits.Object);
        }
        private void SetupAllLimits()
        {
            _limits.Setup(l => l.GetC(It.IsAny<bool>())).Returns(10);
            _limits.Setup(l => l.GetNMC(It.IsAny<bool>())).Returns(50);
            _limits.Setup(l => l.GetPER(It.IsAny<bool>())).Returns(30);
            _limits.Setup(l => l.GetD(It.IsAny<bool>())).Returns(10);
            _limits.Setup(l => l.GetL(It.IsAny<bool>())).Returns(6);
            _limits.Setup(l => l.GetDELTA(It.IsAny<bool>())).Returns(1);
            _limits.Setup(l => l.GetNCZ(It.IsAny<bool>())).Returns(10);
            _limits.Setup(l => l.GetPERSIMP()).Returns(10);
            _limits.Setup(l => l.GetLIM(It.IsAny<bool>())).Returns(10);
        }

        private BookCopy CreateCopy(int id, string domain)
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
                        new Category { Name = domain }
                    }
                }
            };
        }

        private Rent CreateRentWithCopies(params BookCopy[] copies)
        {
            return new Rent
            {
                StartDate = DateTime.Now.AddDays(-5),
                BookCopies = copies.ToList()
            };
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_Exceeds_C()
        {
            _limits.Setup(l => l.GetC(false)).Returns(2);

            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A"),
                CreateCopy(3, "A")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A", "A" });
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_3BooksButOnly1Domain()
        {
            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A"),
                CreateCopy(3, "A")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A", "A" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_3BooksAnd2Domains()
        {
            _limits.Setup(l => l.GetC(false)).Returns(5);
            _limits.Setup(l => l.GetNMC(false)).Returns(10);
            _limits.Setup(l => l.GetPER(false)).Returns(30);
            _limits.Setup(l => l.GetD(false)).Returns(20);
            _limits.Setup(l => l.GetL(false)).Returns(3);
            _limits.Setup(l => l.GetDELTA(false)).Returns(90);
            _limits.Setup(l => l.GetNCZ(false)).Returns(10);
            _limits.Setup(l => l.GetLIM(false)).Returns(5);

            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A"),
                CreateCopy(3, "B")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A", "B" });
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_Exceeds_NMC()
        {
            _limits.Setup(l => l.GetNMC(false)).Returns(5);
            _limits.Setup(l => l.GetPER(false)).Returns(30);

            _history.Setup(h => h.GetRentsInPeriod(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Rent>
                {
                    CreateRentWithCopies(CreateCopy(1, "A"), CreateCopy(2, "A"), CreateCopy(3, "A"), CreateCopy(4, "A"))
                });

            var copies = new List<BookCopy> { CreateCopy(5, "A"), CreateCopy(6, "A") };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A" });
        }

   
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_Exceeds_DomainLimit()
        {
            _limits.Setup(l => l.GetD(false)).Returns(3);
            _limits.Setup(l => l.GetL(false)).Returns(3);

            _history.Setup(h => h.GetRentsInPeriod(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Rent>
                {
                    CreateRentWithCopies(CreateCopy(1, "A"), CreateCopy(2, "A"))
                });

            var copies = new List<BookCopy>
            {
                CreateCopy(3, "A"),
                CreateCopy(4, "A")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A" });
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_DeltaRuleViolated()
        {
            _limits.Setup(l => l.GetDELTA(false)).Returns(4);

            _history.Setup(h => h.GetRentsForBook(1, 10))
                .Returns(new List<Rent>
                {
                    new Rent { StartDate = DateTime.Now.AddDays(-5) }
                });

            var copies = new List<BookCopy> { CreateCopy(10, "A") };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A" });
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_Exceeds_DailyLimit()
        {
            SetupAllLimits();
            _limits.Setup(l => l.GetNCZ(false)).Returns(3);

            _history.Setup(h => h.GetRentsForDay(1, DateTime.Today))
                .Returns(new List<Rent>
                {
                    CreateRentWithCopies(CreateCopy(1, "A"), CreateCopy(2, "A"))
                });

            var copies = new List<BookCopy>
            {
                CreateCopy(3, "B")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A","B"});
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_Exceeds_ExtensionLimit()
        {
            _limits.Setup(l => l.GetLIM(false)).Returns(2);

            _history.Setup(h => h.GetExtensionsInLast3Months(1))
                .Returns(new List<Rent>
                {
                    new Rent { NumberOfExtensions = 2 }
                });

            var copies = new List<BookCopy> { CreateCopy(1, "A") };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A" });
        }

    
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_Employee_Exceeds_PERSIMP()
        {
            _limits.Setup(l => l.GetPERSIMP()).Returns(2);

            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A"),
                CreateCopy(3, "A")
            };

            _service.ValidateRentRequest(1, true, copies, new List<string> { "A", "A", "A" });
        }
        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_BooksUnderLimit()
        {
            SetupAllLimits();

            _limits.Setup(l => l.GetC(false)).Returns(5);

            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_LessThan3Books()
        {
            SetupAllLimits();
            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A" });
        }
        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_PeriodLimitNotExceeded()
        {
            SetupAllLimits();
            _limits.Setup(l => l.GetNMC(false)).Returns(10);
            _limits.Setup(l => l.GetPER(false)).Returns(30);

            _history.Setup(h => h.GetRentsInPeriod(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Rent>());

            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_DomainLimitNotExceeded()
        {
            SetupAllLimits();
            _limits.Setup(l => l.GetD(false)).Returns(5);
            _limits.Setup(l => l.GetL(false)).Returns(3);

            _history.Setup(h => h.GetRentsInPeriod(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Rent>()); 

            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A")
            };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_DeltaNotViolated()
        {
            SetupAllLimits();
            _limits.Setup(l => l.GetDELTA(false)).Returns(10);

            _history.Setup(h => h.GetRentsForBook(1, 1))
                .Returns(new List<Rent>
                {
            new Rent { StartDate = DateTime.Now.AddDays(-20) } 
                });

            var copies = new List<BookCopy> { CreateCopy(1, "A") };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A" });
        }


        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_Employee_SkipsDailyLimit()
        {
            SetupAllLimits();
            var copies = new List<BookCopy> { CreateCopy(1, "A") };

            _service.ValidateRentRequest(1, true, copies, new List<string> { "A" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_DailyLimitNotExceeded()
        {
            SetupAllLimits();
            _limits.Setup(l => l.GetNCZ(false)).Returns(5);

            _history.Setup(h => h.GetRentsForDay(1, DateTime.Today))
                .Returns(new List<Rent>()); 

            var copies = new List<BookCopy> { CreateCopy(1, "A"), CreateCopy(2, "A"), CreateCopy(3, "B") };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A","B" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_EmployeeUnderProcessingLimit()
        {
            SetupAllLimits();
            _limits.Setup(l => l.GetPERSIMP()).Returns(5);

            var copies = new List<BookCopy>
            {
                CreateCopy(1, "A"),
                CreateCopy(2, "A")
            };

            _service.ValidateRentRequest(1, true, copies, new List<string> { "A", "A" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_Reader_SkipsEmployeeLimit()
        {
            SetupAllLimits();
            var copies = new List<BookCopy> { CreateCopy(1, "A") };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A" });
        }

        [TestMethod]
        public void ValidateRentRequest_Should_Pass_When_ExtensionsUnderLimit()
        {
            SetupAllLimits();
            _limits.Setup(l => l.GetLIM(false)).Returns(5);

            _history.Setup(h => h.GetExtensionsInLast3Months(1))
                .Returns(new List<Rent>
                {
            new Rent { NumberOfExtensions = 1 }
                });

            var copies = new List<BookCopy> { CreateCopy(1, "A") };

            _service.ValidateRentRequest(1, false, copies, new List<string> { "A" });
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateMaxBooksPerPeriod_Should_Throw_When_Exceeds_NMC()
        {

            var limits = new Mock<IRentLimitService>();
            var history = new Mock<IRentHistoryService>();

            limits.Setup(l => l.GetNMC(false)).Returns(5);  
            limits.Setup(l => l.GetPER(false)).Returns(30);  

            history.Setup(h => h.GetRentsInPeriod(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                   .Returns(new List<Rent>
                   {
               new Rent { BookCopies = new List<BookCopy> { new BookCopy(), new BookCopy(), new BookCopy(), new BookCopy() } }
                   });

            var service = new RentRulesService(history.Object, limits.Object);

            var copies = new List<BookCopy> { new BookCopy(), new BookCopy() };

            service.ValidateRentRequest(1, false, copies, new List<string> { "A", "A" });
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidateRentRequest_Should_Throw_When_Exceeds_DomainLimit_In_ValidateMaxBooksPerDomain()
        {
            var history = new Mock<IRentHistoryService>();
            var limits = new Mock<IRentLimitService>();

            limits.Setup(l => l.GetD(false)).Returns(3);

            limits.Setup(l => l.GetL(false)).Returns(3);

            history.Setup(h => h.GetRentsInPeriod(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                   .Returns(new List<Rent>
                   {
                       new Rent
                       {
                           BookCopies = new List<BookCopy>
                           {
                               new BookCopy { Book = new Book { Categories = new List<Category> { new Category { Name = "A" } } } },
                               new BookCopy { Book = new Book { Categories = new List<Category> { new Category { Name = "A" } } } },
                               new BookCopy { Book = new Book { Categories = new List<Category> { new Category { Name = "A" } } } }
                           }
                       }
                   });

            var service = new RentRulesService(history.Object, limits.Object);

            var copies = new List<BookCopy>
            {
                new BookCopy { Book = new Book { Categories = new List<Category> { new Category { Name = "A" } } } }
            };

            var domains = new List<string> { "A" };

            service.ValidateRentRequest(1, false, copies, domains);
        }

    }
}
