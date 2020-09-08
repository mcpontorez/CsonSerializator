using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wild.Cson.Serialization;

namespace SerializatorApp
{
    public class AnyBenchmark
    {
        //private static List<string> _list = new List<string>();
        //private static object _listAsObject = _list;
        //private static Type _listType = _list.GetType();

        //public bool _result1, _result2;

        //[Benchmark()]
        //public void IsType()
        //{
        //    _result1 = _list is IList;
        //}

        //[Benchmark()]
        //public void AsObjectIsType()
        //{
        //    _result1 = _listAsObject is IList;
        //}

        //[Benchmark()]
        //public void ReflectionIsType()
        //{
        //    _result2 = typeof(IList).IsAssignableFrom(_listType);
        //}

        //[Benchmark()]
        //public void TestGetType()
        //{
        //    _listType = _list.GetType();
        //}

        //[Benchmark()]
        //public void TestObjectGetType()
        //{
        //    _listType = _listAsObject.GetType();
        //}

        public static string Str1 = "geewdWQE", Str2 = "teewdWQE";
        public static bool Result;

        [Benchmark]
        public void RavnoFirstItemStr()
        {
            Result = Str1[0] == Str2[0] && Str1 == Str2;
        }

        [Benchmark]
        public void RavnoStr()
        {
            Result = Str1 == Str2;
        }

        //private static object o = new Dictionary<string, List<DoublePerson>>();

        //[Benchmark()]
        //public void TestGetType()
        //{
        //    o.GetType();
        //    o.GetType();
        //    o.GetType();
        //}
    }

    [MemoryDiagnoser]
    public class SerializationUtilsBenchmark
    {
        [Benchmark]
        public void SerializationCson()
        {
            CsonUtil.To(TestData.Instance);
        }

        [Benchmark(Baseline = true)]
        public void SerializationJson()
        {
            JsonConvert.SerializeObject(TestData.Instance, new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.All });
        }

        [Benchmark()]
        public void DeserializationJson()
        {
            JsonConvert.DeserializeObject<object>(TestData.InstanceJson);
        }

        [Benchmark()]
        public void DeserializationCson()
        {
            CsonUtil.From<DoublePerson>(TestData.InstanceCson);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var converter = new Wild.Cson.Serialization.Serializators.Converters.MainConverter();
            //string cson = converter.Convert(TestData.Instance);
            //Console.WriteLine(cson);

            //var deserializator = new Wild.Cson.Serialization.Deserializators.Converters.MainConverter();
            //DoublePerson desDoublePerson = deserializator.Convert<DoublePerson>(cson);

            //string cson2 = converter.Convert(desDoublePerson);
            //Console.WriteLine(cson2);

            //Console.WriteLine(Equals(TestData.Instance, desDoublePerson));
            //Console.WriteLine(cson == cson2);

            Console.WriteLine(TestData.InstanceCson);
            BenchmarkRunner.Run<SerializationUtilsBenchmark>();
            //BenchmarkRunner.Run<AnyBenchmark>();

            //var b = new SerializationUtilsBenchmark();
            //for (int i = 0; i < 10000; i++)
            //{
            //    b.DeserializationCson();
            //}

            Console.Read();
        }
    }
}
