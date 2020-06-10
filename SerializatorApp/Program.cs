using SerializatorApp.Serialization.Converters;
using SerializatorApp.Serialization.Deserializators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Super
{
    public class Person
    {
        public float SuperId;
        public string SuperName;

        public object @object = "lalala";
    }
}

namespace SerializatorApp
{
    class Size { public float x, y; }
    class Person
    {
        public int Id;
        public string Name;
        public Size Size;
    }

    class DoublePerson
    {
        public Super.Person SuperPerson;
        public Person SimplePerson;
        public string Description;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person simplePerson = new Person
            { 
                Id = 12, 
                Name = "Boris", 
                Size = new Size { x = 10F, y = 15F } 
            };

            DoublePerson doublePerson = new DoublePerson { SuperPerson = new Super.Person { SuperId = 99, SuperName = "SuperBoris" }, SimplePerson = simplePerson };

            ICsonConverterBase converter = new MainConverter();
            string cson = converter.To(doublePerson).Cson;
            Console.WriteLine(cson);

            IConverterBase deserializator = new ConverterResolver();
            DoublePerson desDoublePerson = deserializator.From<DoublePerson>(new Serialization.Deserializators.StringReader(cson));

            Console.Read();
        }
    }
}
