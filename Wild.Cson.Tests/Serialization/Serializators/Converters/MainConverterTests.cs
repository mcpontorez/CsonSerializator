using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wild.Cson.Serialization.Serializators.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Wild.Cson.Tests.Datas;

namespace Wild.Cson.Serialization.Serializators.Converters.Tests
{
    [TestClass()]
    public class MainConverterTests
    {
        public class Super { };

        private MainConverter _mainConverter = new MainConverter();
        private T Evaluate<T>(string cson) => CSharpScript.EvaluateAsync<T>(cson, Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default.AddReferences(typeof(T).Assembly)).Result;
        private object Evaluate(string cson) => Evaluate<object>(cson);

        public TestContext TestContext { get; set; }

        [TestMethod()]
        public void ConvertSimplePositiveInt32Test()
        {
            int input = new Random().Next(0, int.MaxValue), expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            int actual = CSharpScript.EvaluateAsync<int>(cson).Result;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertSimpleNegativeInt32Test()
        {
            int input = new Random().Next(int.MinValue, 0), expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            int actual = CSharpScript.EvaluateAsync<int>(cson).Result;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertReferenceTypeNullTest()
        {
            object input = null, expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            object actual = Evaluate(cson);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertNullableValueTypeNullTest()
        {
            int? input = null, expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            int? actual = Evaluate<int?>(cson);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertReferenceTypeTest()
        {
            Person input = DataFactory.GetPerson(), expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            Person actual = Evaluate<Person>(cson);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertValueTypeTest()
        {
            Size input = DataFactory.GetSize(), expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            Size actual = Evaluate<Size>(cson);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ConvertSystemObjectTest()
        {
            object input = new object(), expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            object actual = Evaluate(cson);

            Assert.IsNotNull(actual);
            Assert.AreSame(expected.GetType(), actual.GetType());
        }

        [TestMethod()]
        public void ConvertCommandListTest()
        {
            List<CommandBase> input = DataFactory.GetCommandList(), expected = input;

            string cson = _mainConverter.Convert(input);
            TestContext.WriteLine(cson);
            var actual = Evaluate<List<CommandBase>>(cson);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}