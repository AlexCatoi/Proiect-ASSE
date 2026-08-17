using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProiectASSE.Services.RentService;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RentLimitServiceTests
    {
        private RentLimitService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new RentLimitService(
                nmc: 10,
                per: 30,
                c: 5,
                d: 20,
                l: 3,
                lim: 5,
                delta: 90,
                ncz: 3,
                persimp: 10
            );
        }

        [TestMethod]
        public void NMC_Should_Double_For_Employee()
        {
            Assert.AreEqual(20, _service.GetNMC(true));  // NMC = 10
            Assert.AreEqual(10, _service.GetNMC(false));
        }

        [TestMethod]
        public void PER_Should_Halve_For_Employee()
        {
            Assert.AreEqual(15, _service.GetPER(true));  // PER = 30
            Assert.AreEqual(30, _service.GetPER(false));
        }

        [TestMethod]
        public void C_Should_Double_For_Employee()
        {
            Assert.AreEqual(10, _service.GetC(true));  // C = 5
            Assert.AreEqual(5, _service.GetC(false));
        }

        [TestMethod]
        public void D_Should_Double_For_Employee()
        {
            Assert.AreEqual(40, _service.GetD(true));  // D = 20
            Assert.AreEqual(20, _service.GetD(false));
        }

        [TestMethod]
        public void L_Should_Not_Change()
        {
            Assert.AreEqual(3, _service.GetL(true));
            Assert.AreEqual(3, _service.GetL(false));
        }

        [TestMethod]
        public void LIM_Should_Double_For_Employee()
        {
            Assert.AreEqual(10, _service.GetLIM(true));  // LIM = 5
            Assert.AreEqual(5, _service.GetLIM(false));
        }

        [TestMethod]
        public void DELTA_Should_Halve_For_Employee()
        {
            Assert.AreEqual(45, _service.GetDELTA(true));  // DELTA = 90
            Assert.AreEqual(90, _service.GetDELTA(false));
        }

        [TestMethod]
        public void NCZ_Should_Be_Ignored_For_Employee()
        {
            Assert.AreEqual(int.MaxValue, _service.GetNCZ(true));
            Assert.AreEqual(3, _service.GetNCZ(false));  // NCZ = 3
        }
    }
}
