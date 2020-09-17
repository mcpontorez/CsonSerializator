using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wild.Cson.Serialization.Serializators.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Wild.Cson.Serialization.Serializators.Converters.Tests
{
    [TestClass()]
    public class MainConverterTests
    {
        private MainConverter _mainConverter = new MainConverter();

        public TestContext TestContext { get; set; }

        [TestMethod()]
        public void ConvertSimplePositiveIntTest()
        {
            int input = new Random().Next(0, int.MaxValue), expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            int actual = CSharpScript.EvaluateAsync<int>(cson).Result;

            Assert.AreEqual(expected, actual);
        }
    }
}