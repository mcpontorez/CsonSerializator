using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wild.Cson.Serialization;

namespace SerializatorApp.Super
{
    public class Person
    {
        public float SuperId;
        public string SuperName;

        public object @object = "lalala";

        public override bool Equals(object obj) => Equals(obj as Person);
        public bool Equals(Person obj) => obj != null && (base.Equals(obj) || Equals(SuperId, obj.SuperId) && Equals(SuperName, obj.SuperName) && Equals(@object, obj.@object));
    }
}

namespace SerializatorApp
{
    public struct Size { public float x, y; }
    public class Person
    {
        public int Id;
        public string Name;
        public Size Size;

        public override bool Equals(object obj) => Equals(obj as Person);
        public bool Equals(Person obj) => obj != null && (base.Equals(obj) || Equals(Id, obj.Id) && Equals(Name, obj.Name) && Size.Equals(obj.Size));
    }

    public class DoublePerson
    {
        public Person[] Persons;
        public List<Person> PersonList;
        public List<Super.Person>[] SuperPersonLists = new List<Super.Person>[] { new List<Super.Person>() { new Super.Person() }, new List<Super.Person> { new Super.Person { SuperId = 0.034F } } };
        public Dictionary<string, Size> SizeDictionary = new Dictionary<string, Size> { ["s"] = new Size(), ["a"] = new Size() { x = 999.043F } };
        public Super.Person SuperPerson;
        public Person SimplePerson;
        public string Description;

        public override bool Equals(object obj) => Equals(obj as DoublePerson);
        public bool Equals(DoublePerson obj) => obj != null && (base.Equals(obj) || Equals(SuperPerson, obj.SuperPerson) && Equals(SimplePerson, obj.SimplePerson) && Equals(Description, obj.Description));
    }

    public static class TestData
    {
        public static readonly DoublePerson Instance;
        public static readonly string InstanceCson;
        public static readonly string InstanceJson;

        static TestData()
        {
            Person simplePerson = new Person
            {
                Id = 12,
                Name = "Swamp thing",
                Size = new Size { x = 10.2F, y = 15F }
            };

            DoublePerson doublePerson = new DoublePerson
            {
                Persons = Enumerable.Repeat(simplePerson, 5).ToArray(),
                PersonList = new List<Person> { new Person(), simplePerson },
                SuperPerson = new Super.Person { SuperId = 99, SuperName = "SuperBoris" },
                SimplePerson = simplePerson
            };

            Instance = doublePerson;
            InstanceCson = CsonUtil.To(doublePerson);
            InstanceJson = JsonConvert.SerializeObject(doublePerson, new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.All });
        }
    }
}
