using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaitService;

namespace Wait.Tests
{
    [TestClass]
    public class PrimeServiceTest
    {
        private readonly PrimeService _primeService;

        public PrimeServiceTest()
        {
            this._primeService = new PrimeService();
        }

        [TestMethod]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            bool result = _primeService.IsPrime(1);
            Assert.IsFalse(result, "1 should is not prime");

        }
    }
}
