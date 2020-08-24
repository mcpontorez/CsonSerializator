using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
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
    public class CsonBenchmark
    {
        [Benchmark()]
        public void Serialization()
        {
            CsonUtil.To(TestData.Instance);
        }
        [Benchmark()]
        public void Deserialization()
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

            BenchmarkRunner.Run<CsonBenchmark>();

            Console.Read();
        }
    }
}
