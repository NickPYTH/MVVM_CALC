using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Calc.Interfaces;
using Calc.Models;

namespace TestCalculator
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestMethod2()
        {
            ICalculate calc = new Calculate();
            var res = calc.Parse(new Expression("10+", calc));
            Assert.AreEqual(res.HasError, true);
        }
        [TestMethod]
        public void TestMethod3()
        {
            ICalculate calc = new Calculate();
            var res = calc.Parse(new Expression("10-5", calc));
            Assert.AreEqual(res.Result, 5.0);
        }
        [TestMethod]
        public void TestMethod4()
        {
            ICalculate calc = new Calculate();
            var res = calc.Parse(new Expression("10*5", calc));
            Assert.AreEqual(res.Result, 50.0);
        }
        [TestMethod]
        public void TestMethod5()
        {
            ICalculate calc = new Calculate();
            var res = calc.Parse(new Expression("10/5", calc));
            Assert.AreEqual(res.Result, 2.0);
        }
        [TestMethod]
        public void TestMethod6()
        {
            ICalculate calc = new Calculate();
            var res = calc.Parse(new Expression("10*(5+2+7+9)", calc));
            Assert.AreEqual(res.Result, 230.0);
        }
        [TestMethod]
        public void TestMethod7()
        {
            ICalculate calc = new Calculate();
            var res = calc.Parse(new Expression("-10*(5+2+7+9)", calc));
            Assert.AreEqual(res.Result, -230.0);
        }
    }
}
