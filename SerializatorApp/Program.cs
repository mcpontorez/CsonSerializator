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
    public class SerializationUtilsBenchmark
    {

        [Benchmark()]
        public void SerializationJson()
        {
            JsonConvert.SerializeObject(TestData.Instance, new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.All });
        }

        [Benchmark()]
        public void SerializationCson()
        {
            CsonUtil.To(TestData.Instance);
        }

        //[Benchmark()]
        //public void DeserializationJson()
        //{
        //    JsonConvert.DeserializeObject<object>(TestData.InstanceJson);
        //}

        //[Benchmark()]
        //public void DeserializationCson()
        //{
        //    CsonUtil.From<DoublePerson>(TestData.InstanceCson);
        //}
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

            //var b = new SerializationUtilsBenchmark();
            //for (int i = 0; i < 10000; i++)
            //{
            //    b.SerializationCson();
            //}
            Console.WriteLine(AppDomain.CurrentDomain.GetAssemblies().Length);
            Console.Read();
        }
    }
}
