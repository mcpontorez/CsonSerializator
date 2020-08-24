using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Wild.Cson.Serialization.Serializators.Writing
{
    public interface ICsonWriter
    {
        ICsonWriter Add(string value);
        ICsonWriter AddMemberName(string value);
        ICsonWriter AddNull();
        ICsonWriter AddNew();
        ICsonWriter AddBeginedBrace();
        ICsonWriter AddEndedBrace();
        ICsonWriter AddBeginedAngleBracket();
        ICsonWriter AddEndedAngleBracket();
        ICsonWriter AddBeginedSquareBracket();
        ICsonWriter AddEndedSquareBracket();
        ICsonWriter AddComma();
        ICsonWriter AddEqual();

        ICsonWriter Add(object value);

        ICsonWriter AddType(TypeInfo type);

        ICsonWriter AddLine();
        ICsonWriter AddSpace();

        ICsonWriter AddTabLevel();
        ICsonWriter RemoveTabLevel();

        string GetString();
    }
}