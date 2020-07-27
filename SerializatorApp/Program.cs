using SerializatorApp.Serialization.Deserializators;
using SerializatorApp.Serialization.Serializators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Super
{
    public class Person
    {
        public float SuperId;
        public string SuperName;

        public object @object = "lalala";

        public override bool Equals(object obj) => Equals(obj as Person);
        public bool Equals(Person obj) => base.Equals(obj) || Equals(SuperId, obj.SuperId) && Equals(SuperName, obj.SuperName) && Equals(@object, obj.@object);
    }
}

namespace SerializatorApp
{
    struct Size { public float x, y; }
    class Person
    {
        public int Id;
        public string Name;
        public Size Size;

        public override bool Equals(object obj) => Equals(obj as Person);
        public bool Equals(Person obj) => base.Equals(obj) || Equals(Id, obj.Id) && Equals(Name, obj.Name) && Size.Equals(obj.Size);
    }

    class DoublePerson
    {
        public Super.Person SuperPerson;
        public Person SimplePerson;
        public string Description;

        public override bool Equals(object obj) => Equals(obj as DoublePerson);
        public bool Equals(DoublePerson obj) => base.Equals(obj) || Equals(SuperPerson, obj.SuperPerson) && Equals(SimplePerson, obj.SimplePerson) && Equals(Description, obj.Description);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person simplePerson = new Person
            { 
                Id = 12, 
                Name = "Boris", 
                Size = new Size { x = 10.2F, y = 15F } 
            };

            DoublePerson doublePerson = new DoublePerson { SuperPerson = new Super.Person { SuperId = 99, SuperName = "SuperBoris" }, SimplePerson = simplePerson };

            Serialization.Serializators.IConverterBase converter = new Serialization.Serializators.MainConverter();
            string cson = converter.To(doublePerson).Cson;
            Console.WriteLine(cson);

            Serialization.Deserializators.IConverterBase deserializator = new Serialization.Deserializators.MainConverter();
            DoublePerson desDoublePerson = deserializator.Convert<DoublePerson>(new Serialization.Deserializators.StringReader(cson));

            string cson2 = converter.To(desDoublePerson).Cson;
            Console.WriteLine(cson2);

            Console.WriteLine(Equals(doublePerson, desDoublePerson));
            Console.WriteLine(cson == cson2);
            Console.Read();
            
        }
    }
}
