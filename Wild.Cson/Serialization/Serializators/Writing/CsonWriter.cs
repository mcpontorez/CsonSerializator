using Wild.Cson.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Wild.Cson.Serialization.Serializators.Writing
{
    internal interface IStringPart
    {
        string ToString();
    }
    internal interface ISecondStringPart : IStringPart { }
    internal class StringContainer : IStringPart
    {
        public static readonly StringContainer Null = new StringContainer(StringConsts.Null), New = new StringContainer(StringConsts.New);
        public readonly string Value;
        public StringContainer(string value) => Value = value;

        public override string ToString() => Value;
    }

    internal class CharContainer : IStringPart
    {
        public static readonly CharContainer
            BeginedBrace = new CharContainer(CharConsts.BeginedBrace), EndedBrace = new CharContainer(CharConsts.EndedBrace),
            BeginedAngleBracket = new CharContainer(CharConsts.BeginedAngleBracket), EndedAngleBracket = new CharContainer(CharConsts.EndedAngleBracket),
            BeginedSquareBracket = new CharContainer(CharConsts.BeginedSquareBracket), EndedSquareBracket = new CharContainer(CharConsts.EndedSquareBracket),
            Comma = new CharContainer(CharConsts.Comma),
            Equal = new CharContainer(CharConsts.Equal),
            AtSign = new CharContainer(CharConsts.AtSign);
        public readonly char Value;
        public CharContainer(char value) => Value = value;

        public override string ToString() => Value.ToString();
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
        public static readonly SecondStringContainer NewLine = new SecondStringContainer(Environment.NewLine), Space = new SecondStringContainer(CharConsts.Space.ToString());

        public readonly string Value;
        private SecondStringContainer(string value) => Value = value;

        public override string ToString() => Value;
    }

    internal class TypeContainer : IStringPart
    {
        public readonly TypeInfo Value;
        public TypeContainer(TypeInfo value) => Value = value;

        public override string ToString() => Value.ToString();
        public void ToString(StringBuilder stringBuilder, bool isFullTypeName)
        {
            string typeName = isFullTypeName ? Value.FullName : Value.Name;
            
            if (!Value.IsGenericType)
            {
                stringBuilder.Append(typeName);
                return;
            }
            int endIndex = typeName.LastIndexOf('`');
            stringBuilder.Append(typeName, 0, endIndex);
        }
    }

    public sealed class CsonWriter : ICsonWriter
    {
        private ITypeService _typeService = new TypeService();

        private List<IStringPart> _stringParts = new List<IStringPart>();

        public bool IsTiny { get; } = false;

        public CsonWriter() { }
        public CsonWriter(bool isTiny) => IsTiny = isTiny;

        private ICsonWriter Add(IStringPart value)
        {
            _stringParts.Add(value);
            return this;
        }
        private ICsonWriter Add(ISecondStringPart value)
        {
            if(!IsTiny)
                _stringParts.Add(value);
            return this;
        }

        public ICsonWriter Add(string value) => Add(new StringContainer(value));
        public ICsonWriter AddMemberName(string value)
        {
            if (StringHelper.IsKeyword(value))
                Add(CharContainer.AtSign);
            return Add(new StringContainer(value));
        }

        public ICsonWriter AddNull() => Add(StringContainer.Null);
        public ICsonWriter AddNew() => Add(StringContainer.New);
        public ICsonWriter AddBeginedBrace() => Add(CharContainer.BeginedBrace);
        public ICsonWriter AddEndedBrace() => Add(CharContainer.EndedBrace);
        public ICsonWriter AddBeginedAngleBracket() => Add(CharContainer.BeginedAngleBracket);
        public ICsonWriter AddEndedAngleBracket() => Add(CharContainer.EndedAngleBracket);
        public ICsonWriter AddBeginedSquareBracket() => Add(CharContainer.BeginedSquareBracket);
        public ICsonWriter AddEndedSquareBracket() => Add(CharContainer.EndedSquareBracket);
        public ICsonWriter AddComma() => Add(CharContainer.Comma);
        public ICsonWriter AddEqual() => Add(CharContainer.Equal);

        public ICsonWriter Add(object value) => Add(value.ToString());

        public ICsonWriter AddType(TypeInfo type)
        {
            if (type.IsArray)
                return AddType(type.GetElementType().GetTypeInfo()).AddBeginedSquareBracket().AddEndedSquareBracket();

            _typeService.Add(type);
            Add(new TypeContainer(type));

            if (!type.IsGenericType)
                return this;

            AddBeginedAngleBracket();
            Type[] genericTypeArguments = type.GenericTypeArguments;
            for (int i = 0; i < genericTypeArguments.Length; i++)
            {
                if (i > 0)
                    AddComma().AddSpace();
                AddType(genericTypeArguments[i].GetTypeInfo());
            }
            AddEndedAngleBracket();
            return this;
        }

        public ICsonWriter AddLine() => Add(SecondStringContainer.NewLine);
        public ICsonWriter AddSpace() => Add(SecondStringContainer.Space);

        public ICsonWriter AddTabLevel(int value) => Add(new TabLevelContainer(value));

        public ICsonWriter AddTabLevel() => Add(TabLevelContainer.OneTab);

        public ICsonWriter RemoveTabLevel() => Add(TabLevelContainer.MinusOneTab);

        public string GetString()
        {
            TypesData typesData = _typeService.GetTypesData();

            StringBuilder stringBuilder = new StringBuilder((20 * typesData.Namespaces.Count) + _stringParts.Count);

            foreach (var @namespace in typesData.Namespaces)
            {
                stringBuilder.Append(StringConsts.Using).Append(@namespace).Append(CharConsts.Semicolon);
                if (!IsTiny)
                    stringBuilder.AppendLine();
            }

            int tabLevel = 0;

            foreach (var item in _stringParts)
            {
                switch (item)
                {
                    case CharContainer c:
                        stringBuilder.Append(c.Value);
                        break;
                    case StringContainer s:
                        stringBuilder.Append(s.Value);
                        break;
                    case TypeContainer tc:
                        tc.ToString(stringBuilder, typesData.IsWritesFullNames[tc.Value]);
                        break;
                    case SecondStringContainer ssc:
                        stringBuilder.Append(ssc.Value);
                        if(ssc == SecondStringContainer.NewLine)
                            stringBuilder.Append(TabLevelContainer.Tab, tabLevel);
                        break;
                    case TabLevelContainer tlc:
                        tabLevel += tlc.Value;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            stringBuilder.Append(CharConsts.Semicolon);
            return stringBuilder.ToString();
        }
    }
}