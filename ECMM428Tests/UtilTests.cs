using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECMM428;

namespace ECMM428Tests
{
    [TestClass]
    public class UtilTests
    {
        [TestMethod]
        public void GetAverageTest()
        {
            double[] array = new double[] { 0, 0, 1, 10, 21 };
            double average = Util.GetAverage(array);
            Assert.AreEqual(6.4, average, 0);
        }
    }
}
