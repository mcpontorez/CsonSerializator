using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Tests.Datas
{
    public struct Size
    {
        public float X, Y;
    }

    public class Person
    {
        public int Id;
        public string Name;
    }

    public abstract class CommandBase { public virtual void Do() { } }

    public abstract class CommandBase<TData> : CommandBase
    {
        public TData Data;
    }

    public class ObjectCommand : CommandBase<object> { };

    public class PersonCommand : CommandBase<Person> { };
}
namespace Wild.Cson.Tests.Datas.Super
{
    public class Person
    {
        public int Id;
        public string SuperName;
        public Size Size;
    }

    public class PersonCommand : CommandBase<Person> { };
}

namespace Wild.Cson.Tests.Datas
{
    public class CommandEnumerable
    {
        public IEnumerable<CommandBase> Items;
    }

    public static class DatasFactory
    {
        public static Size GetSize() => new Size { X = -12.1F, Y = 0.1F };
        public static Person GetPerson() => new Person { Id = 12, Name = "Simple cat" };
        public static Super.Person GetSuperPerson() => new Super.Person { Id = 99, SuperName = "Super cat", Size = GetSize() };
        public static ObjectCommand GetObjectCommand() => new ObjectCommand() { Data = new object() };
        public static PersonCommand GetPersonCommand() => new PersonCommand { Data = GetPerson() };
        public static Super.PersonCommand GetSuperPersonCommand() => new Super.PersonCommand { Data = GetSuperPerson() };

        public static CommandBase[] GetCommandArray() => new CommandBase[] { GetObjectCommand(), GetPersonCommand(), GetSuperPersonCommand() };
        public static List<CommandBase> GetCommandList() => GetCommandArray().ToList();
        public static Dictionary<string, CommandBase> GetCommandDictionary()
        {
            int index = 0;
            return GetCommandArray().ToDictionary(c => "key" + index++);
        }

        public static CommandEnumerable GetCommandEnumerableArray() => new CommandEnumerable { Items = GetCommandArray() };
        public static CommandEnumerable GetCommandEnumerableList() => new CommandEnumerable { Items = GetCommandList() };
    }
}
