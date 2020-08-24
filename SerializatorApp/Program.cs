using System;
using System.Collections;
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
        public Person[] Persons;
        public List<Person> PersonList;
        public List<Super.Person>[] SuperPersonLists = new List<Super.Person>[] { new List<Super.Person>() { new Super.Person() }, new List<Super.Person> { new Super.Person { SuperId = 0.034F } } };
        public Dictionary<string, Size> SizeDictionary = new Dictionary<string, Size> { ["s"] = new Size(), ["a"] = new Size() { x = 999.043F } };
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

            DoublePerson doublePerson = new DoublePerson { Persons = new Person[]{ new Person() }, PersonList = new List<Person> { new Person() },
                SuperPerson = new Super.Person { SuperId = 99, SuperName = "SuperBoris" }, SimplePerson = simplePerson };

            var converter = new Wild.Cson.Serialization.Serializators.Converters.MainConverter();
            string cson = converter.Convert(doublePerson);
            Console.WriteLine(cson);

            var deserializator = new Wild.Cson.Serialization.Deserializators.Converters.MainConverter();
            DoublePerson desDoublePerson = deserializator.Convert<DoublePerson>(cson);

            string cson2 = converter.Convert(desDoublePerson);
            Console.WriteLine(cson2);

            Console.WriteLine(Equals(doublePerson, desDoublePerson));
            Console.WriteLine(cson == cson2);
            Console.Read();

            //string str = "\"\\ \r\n a";
        }
    }
}
