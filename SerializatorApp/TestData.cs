using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wild.Cson.Serialization;

namespace SerializatorApp.Super
{
    public class Person : IEquatable<Person>
    {
        public float SuperId;
        public string SuperName;

        public object @object = "lalala";

        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public bool Equals(Person other)
        {
            return other != null &&
                   SuperId == other.SuperId &&
                   SuperName == other.SuperName &&
                   EqualityComparer<object>.Default.Equals(@object, other.@object);
        }

        public static bool operator ==(Person left, Person right)
        {
            return left?.Equals(right) ?? ReferenceEquals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }
    }
}

namespace SerializatorApp
{
    public struct Size : IEquatable<Size>
    {
        public float X, Y;

        public override bool Equals(object obj)
        {
            return obj is Size size && Equals(size);
        }

        public bool Equals(Size other)
        {
            bool result = Math.Abs(X - other.X) <= float.Epsilon &&
                   Math.Abs(Y - other.Y) <= float.Epsilon;
            if(!result)
                return false;
            return result;
        }

        public static bool operator ==(Size left, Size right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !(left == right);
        }
    }
    public class Person : IEquatable<Person>
    {
        public int Id;
        public string Name;
        public Size Size;

        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public bool Equals(Person other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Size.Equals(other.Size);
        }

        public static bool operator ==(Person left, Person right)
        {
            return left?.Equals(right) ?? ReferenceEquals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }
    }

    public class DoublePerson : IEquatable<DoublePerson>
    {
        public Person[] Persons;
        public HashSet<string> DescriptionHashSet;
        public List<Person> PersonList;
        public List<Super.Person>[] SuperPersonLists;
        public Dictionary<string, Size> SizeDictionary;
        public Super.Person SuperPerson;
        public Person SimplePerson;
        public string Description;

        public override bool Equals(object obj)
        {
            return Equals(obj as DoublePerson);
        }

        public bool Equals(DoublePerson other)
        {
            return other != null &&
                   Persons.EnumerableEquals(other.Persons) &&
                   DescriptionHashSet.EnumerableEquals(other.DescriptionHashSet) &&
                   PersonList.EnumerableEquals(other.PersonList) &&
                   SuperPersonLists.EnumerableEquals(other.SuperPersonLists) &&
                   SizeDictionary.EnumerableEquals(other.SizeDictionary) &&
                   SuperPerson.Equals(other.SuperPerson) &&
                   SimplePerson.Equals(other.SimplePerson) &&
                   Description == other.Description;
        }

        public static bool operator ==(DoublePerson left, DoublePerson right)
        {
            return left?.Equals(right) ?? ReferenceEquals(left, right);
        }

        public static bool operator !=(DoublePerson left, DoublePerson right)
        {
            return !(left == right);
        }
    }

    public class AllDoublePersons : IEquatable<AllDoublePersons>
    {
        public List<DoublePerson> DoublePersons;

        public override bool Equals(object obj)
        {
            return Equals(obj as AllDoublePersons);
        }

        public bool Equals(AllDoublePersons other)
        {
            return other != null &&
                   DoublePersons.EnumerableEquals(other.DoublePersons);
        }

        public static bool operator ==(AllDoublePersons left, AllDoublePersons right)
        {
            return left?.Equals(right) ?? ReferenceEquals(left, right);
        }

        public static bool operator !=(AllDoublePersons left, AllDoublePersons right)
        {
            return !(left == right);
        }
    }

    public static class TestData
    {
        public static readonly AllDoublePersons Instance;
        public static readonly string InstanceCson;
        public static readonly string InstanceJson;

        static TestData()
        {
            Person simplePerson = new Person
            {
                Id = 12,
                Name = "Swamp thing",
                Size = new Size { X = 10.2F, Y = -.15F }
            };

            DoublePerson doublePerson = new DoublePerson
            {
                Persons = Enumerable.Repeat(simplePerson, 5).ToArray(),
                DescriptionHashSet = new HashSet<string> { "Super", "Duper", "super", "puper" },
                PersonList = new List<Person> { new Person(), simplePerson },
                SuperPersonLists = new List<Super.Person>[] { new List<Super.Person>() { new Super.Person() }, new List<Super.Person> { new Super.Person { SuperId = 0.034F } } },
                SizeDictionary = new Dictionary<string, Size> { ["s"] = new Size(), ["a"] = new Size() { X = 999.043F } },
                SuperPerson = new Super.Person { SuperId = 99, SuperName = "blet cat" },
                SimplePerson = simplePerson,
            };
            AllDoublePersons allDoublePersons = new AllDoublePersons { DoublePersons = Enumerable.Repeat(doublePerson, 20).ToList() };
            Instance = allDoublePersons;
            InstanceCson = CsonUtil.To(Instance);
            AllDoublePersons desAllDoublePersons = CsonUtil.From<AllDoublePersons>(InstanceCson);
            Console.WriteLine(Instance.Equals(JsonConvert.DeserializeObject<AllDoublePersons>(JsonConvert.SerializeObject(Instance, new JsonSerializerSettings()))));
            Console.WriteLine(Instance.Equals(desAllDoublePersons));

            SaveCson();

            Console.WriteLine(InstanceCson);
            Console.WriteLine(CsonUtil.To(desAllDoublePersons));

            InstanceJson = JsonConvert.SerializeObject(Instance, new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.All });
        }

        private static void SaveCson()
        {
            string cson1 = InstanceCson, cson2 = CsonUtil.To(CsonUtil.From<AllDoublePersons>(cson1));
            FileInfo cson1File = new FileInfo("cson1.cson"), cson2File = new FileInfo("cson2.cson");
            File.WriteAllText(cson1File.FullName, cson1);
            File.WriteAllText(cson2File.FullName, cson2);
        }

        public static bool EnumerableEquals<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            T[] s = source.ToArray(), o = other.ToArray();
            for (int i = 0; i < s.Length; i++)
            {
                if (!o.Contains(s[i]))
                    return false;
            }
            return true;
        }
    }
}
