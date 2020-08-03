using SerializatorApp.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SerializatorApp.Serialization.Serializators.Writing
{
    internal interface IStringPart
    {
        string ToString();
    }
    internal interface ISecondStringPart : IStringPart { }
    internal class StringContainer : IStringPart
    {
        public static readonly StringContainer Null = new StringContainer(StringConsts.Null), New = new StringContainer(StringConsts.New),
            BeginedBrace = new StringContainer(StringConsts.BeginedBrace), EndedBrace = new StringContainer(StringConsts.EndedBrace),
            Comma = new StringContainer(StringConsts.Comma),
            Equal = new StringContainer(StringConsts.Equal),
            AtSign = new StringContainer(StringConsts.AtSign);
        public readonly string Value;
        public StringContainer(string value) => Value = value;

        public override string ToString() => Value;
    }

    internal class TabLevelContainer : ISecondStringPart
    {
        public static readonly char Tab = '\t';

        public static readonly TabLevelContainer OneTab = new TabLevelContainer(1), MinusOneTab = new TabLevelContainer(-1);

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

    internal class SecondStringContainer : ISecondStringPart
    {
        public static readonly SecondStringContainer NewLine = new SecondStringContainer(Environment.NewLine), Space = new SecondStringContainer(StringConsts.Space);

        public readonly string Value;
        private SecondStringContainer(string value) => Value = value;

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

        public bool IsTiny { get; } = false;

        public StringWriter() { }
        public StringWriter(bool isTiny) => IsTiny = isTiny;

        private IStringWriter Add(IStringPart value)
        {
            _stringParts.Add(value);
            return this;
        }
        private IStringWriter Add(ISecondStringPart value)
        {
            if(!IsTiny)
                _stringParts.Add(value);
            return this;
        }

        public IStringWriter Add(string value) => Add(new StringContainer(value));
        public IStringWriter AddMemberName(string value)
        {
            if (StringHelper.IsKeyword(value))
                Add(StringContainer.AtSign);
            return Add(new StringContainer(value));
        }

        public IStringWriter AddNull() => Add(StringContainer.Null);
        public IStringWriter AddNew() => Add(StringContainer.New);
        public IStringWriter AddBeginedBrace() => Add(StringContainer.BeginedBrace);
        public IStringWriter AddEndedBrace() => Add(StringContainer.EndedBrace);
        public IStringWriter AddComma() => Add(StringContainer.Comma);
        public IStringWriter AddEqual() => Add(StringContainer.Equal);

        public IStringWriter Add(object value) => Add(value.ToString());

        public IStringWriter AddType(Type type)
        {
            _typeService.Add(type);
            Add(new TypeContainer(type));
            return this;
        }

        public IStringWriter AddLine() => Add(SecondStringContainer.NewLine);
        public IStringWriter AddSpace() => Add(SecondStringContainer.Space);

        public IStringWriter AddTabLevel(int value) => Add(new TabLevelContainer(value));

        public IStringWriter AddTabLevel() => Add(TabLevelContainer.OneTab);

        public IStringWriter RemoveTabLevel() => Add(TabLevelContainer.MinusOneTab);

        public string GetString()
        {
            TypesData typesData = _typeService.GetTypesData();

            StringBuilder stringBuilder = new StringBuilder((10 * typesData.Namespaces.Count) + _stringParts.Count);

            foreach (var @namespace in typesData.Namespaces)
            {
                stringBuilder.Append(StringConsts.Using).Append(@namespace).Append(StringConsts.Semicolon);
                if (!IsTiny)
                    stringBuilder.AppendLine();
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
                    case SecondStringContainer ssc:
                        stringBuilder.Append(ssc.Value);
                        if(ssc == SecondStringContainer.NewLine)
                            for (int i = 0; i < tabLevel; i++)
                                stringBuilder.Append(TabLevelContainer.Tab);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            stringBuilder.Append(StringConsts.Semicolon);
            return stringBuilder.ToString();
        }
    }
}