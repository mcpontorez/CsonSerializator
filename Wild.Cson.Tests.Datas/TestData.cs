using System;
using System.Collections.Generic;
using System.Linq;

namespace Wild.Cson.Tests.Datas
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
        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public bool Equals(Person other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name;
        }

        public static bool operator ==(Person left, Person right)
        {
            return EqualityComparer<Person>.Default.Equals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }
    }

    public abstract class CommandBase { public virtual void Do() { } }

    public abstract class CommandBase<TData> : CommandBase, IEquatable<CommandBase<TData>>
    {
        public TData Data;
        public override bool Equals(object obj)
        {
            return Equals(obj as CommandBase<TData>);
        }

        public bool Equals(CommandBase<TData> other)
        {
            return other != null &&
                   EqualityComparer<TData>.Default.Equals(Data, other.Data);
        }

        public static bool operator ==(CommandBase<TData> left, CommandBase<TData> right)
        {
            return EqualityComparer<CommandBase<TData>>.Default.Equals(left, right);
        }

        public static bool operator !=(CommandBase<TData> left, CommandBase<TData> right)
        {
            return !(left == right);
        }
    }

    public class ObjectCommand : CommandBase<object> { };

    public class PersonCommand : CommandBase<Person> { };
}
namespace Wild.Cson.Tests.Datas.Super
{
    public class Person : IEquatable<Person>
    {
        public int Id;
        public string SuperName;
        public Size Size;
        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public bool Equals(Person other)
        {
            return other != null &&
                   Id == other.Id &&
                   SuperName == other.SuperName &&
                   Size.Equals(other.Size);
        }

        public static bool operator ==(Person left, Person right)
        {
            return EqualityComparer<Person>.Default.Equals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }
    }

    public class PersonCommand : CommandBase<Person> { };
}

namespace Wild.Cson.Tests.Datas
{
    public class CommandEnumerable : IEquatable<CommandEnumerable>
    {
        public IEnumerable<CommandBase> Items;
        public override bool Equals(object obj)
        {
            return Equals(obj as CommandEnumerable);
        }

        public bool Equals(CommandEnumerable other)
        {
            return other != null &&
                   EqualityComparer<IEnumerable<CommandBase>>.Default.Equals(Items, other.Items);
        }

        public static bool operator ==(CommandEnumerable left, CommandEnumerable right)
        {
            return EqualityComparer<CommandEnumerable>.Default.Equals(left, right);
        }

        public static bool operator !=(CommandEnumerable left, CommandEnumerable right)
        {
            return !(left == right);
        }
    }

    public static class DataFactory
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
