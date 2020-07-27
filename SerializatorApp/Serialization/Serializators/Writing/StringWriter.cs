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
        public readonly string Target;
        public StringContainer(string target) => Target = target;

        public override string ToString() => Target;
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

    public sealed class StringWriter
    {
        private ITypeService _typeService = new TypeService();

        private List<IStringPart> _stringParts = new List<IStringPart>();

        public StringWriter Add(string value)
        {
            _stringParts.Add(new StringContainer(value));
            return this;
        }

        public StringWriter Add(object value) => Add(value.ToString());

        public StringWriter Add(Type type)
        {
            _typeService.Add(type);
            _stringParts.Add(new TypeContainer(type));
            return this;
        }

        public string GetString()
        {
            TypesData typesData = _typeService.GetTypesData();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var @namespace in typesData.Namespaces)
            {
                stringBuilder.Append("using ").Append(@namespace).Append(';').AppendLine();
            }
            foreach (var item in _stringParts)
            {
                string part;
                switch (item)
                {
                    case StringContainer s:
                        part = s.ToString();
                        break;
                    case TypeContainer tc:
                        part = tc.ToString(typesData.IsWritesFullNames[tc.Target]);
                        break;
                    default:
                        throw new ArgumentException();
                }

                stringBuilder.Append(part);
            }

            return stringBuilder.ToString();
        }
    }
}