using SerializatorApp.Serialization.Deserializators.Reading;
using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Deserializators.Converters
{
    public abstract class ConverterBase : IConverter
    {
        protected const char _startObjectChar = '{', _endObjectChar = '}', _endChar = ',', _fullEndChar = ';';
        protected static readonly IReadOnlyList<char> _endChars = new char[]{_endChar, _fullEndChar, _endObjectChar};

        public TResult Convert<TResult>(CsonReader cson) => Convert<TResult>(cson, TypeNameResolver.Empty);

        public abstract TResult Convert<TResult>(CsonReader cson, ITypeNameResolver typeNameResolver);

        public abstract bool IsCanConvertable(CsonReader cson);

        protected bool IsAnyEndChar(char value) => value == _endChar || value == _fullEndChar || value == _endObjectChar;
    }
}
