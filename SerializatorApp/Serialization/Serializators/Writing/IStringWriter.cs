using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SerializatorApp.Serialization.Serializators.Writing
{
    public interface IStringWriter
    {
        IStringWriter Add(string value);
        IStringWriter AddMemberName(string value);
        IStringWriter AddNull();
        IStringWriter AddNew();
        IStringWriter AddBeginedBrace();
        IStringWriter AddEndedBrace();
        IStringWriter AddBeginedAngleBracket();
        IStringWriter AddEndedAngleBracket();
        IStringWriter AddBeginedSquareBracket();
        IStringWriter AddEndedSquareBracket();
        IStringWriter AddComma();
        IStringWriter AddEqual();

        IStringWriter Add(object value);

        IStringWriter AddType(TypeInfo type);

        IStringWriter AddLine();
        IStringWriter AddSpace();

        IStringWriter AddTabLevel();
        IStringWriter RemoveTabLevel();

        string GetString();
    }
}