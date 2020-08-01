using System;
using System.Collections.Generic;
using System.Text;

namespace SerializatorApp.Serialization.Serializators.Writing
{
    internal interface IStringPart
    {
        string ToString();
    }
    internal class StringContainer : IStringPart
    {
        public readonly string Value;
        public StringContainer(string value) => Value = value;

        public override string ToString() => Value;
    }

    internal class TabLevelContainer : IStringPart
    {
        public static readonly char Tab = '\t';

        public readonly int Value;
        public TabLevelContainer(int value) => Value = value;

        public override string ToString()
        {
            char[] result = new char[Value];
            for (int i = 0; i < Value; i++)
                result[i] = Tab;
            return new string(result);
        }
    }

    internal class NewLineContainer : IStringPart
    {
        public static readonly string Value = Environment.NewLine;

        private NewLineContainer() { }

        public static readonly NewLineContainer Instance = new NewLineContainer();

        public override string ToString() => Value;
    }

    internal class TypeContainer : IStringPart
    {
        public readonly Type Target;
        public TypeContainer(Type type)
        {
            Target = type;
        }

        public override string ToString() => Target.ToString();
        public string ToString(bool isFullTypeName) => isFullTypeName ? Target.FullName : Target.Name;
    }

    public sealed class StringWriter : IStringWriter
    {
        private ITypeService _typeService = new TypeService();

        private List<IStringPart> _stringParts = new List<IStringPart>();

        public IStringWriter Add(string value)
        {
            _stringParts.Add(new StringContainer(value));
            return this;
        }

        public IStringWriter Add(object value) => Add(value.ToString());

        public IStringWriter Add(Type type)
        {
            _typeService.Add(type);
            _stringParts.Add(new TypeContainer(type));
            return this;
        }

        public IStringWriter AddLine()
        {
            _stringParts.Add(NewLineContainer.Instance);
            return this;
        }

        public IStringWriter AddTabLevel(int value)
        {
            _stringParts.Add(new TabLevelContainer(value));
            return this;
        }

        public string GetString()
        {
            TypesData typesData = _typeService.GetTypesData();

            StringBuilder stringBuilder = new StringBuilder((10 * typesData.Namespaces.Count) + _stringParts.Count);

            foreach (var @namespace in typesData.Namespaces)
            {
                stringBuilder.Append("using ").Append(@namespace).Append(';').AppendLine();
            }

            int tabLevel = 0;

            foreach (var item in _stringParts)
            {
                switch (item)
                {
                    case StringContainer s:
                        stringBuilder.Append(s.ToString());
                        break;
                    case TypeContainer tc:
                        stringBuilder.Append(tc.ToString(typesData.IsWritesFullNames[tc.Target]));
                        break;
                    case TabLevelContainer tlc:
                        tabLevel += tlc.Value;
                        break;
                    case NewLineContainer nlc:
                        stringBuilder.AppendLine();
                        for (int i = 0; i < tabLevel; i++)
                            stringBuilder.Append(TabLevelContainer.Tab);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            return stringBuilder.ToString();
        }
    }
}