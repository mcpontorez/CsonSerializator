﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SerializatorApp.Serialization.Serializators.Writing
{
    public interface IStringWriter
    {
        IStringWriter Add(string value);
        IStringWriter AddNull();
        IStringWriter AddNew();
        IStringWriter AddBeginedBrace();
        IStringWriter AddEndedBrace();
        IStringWriter AddComma();
        IStringWriter AddEqual();

        IStringWriter Add(object value);

        IStringWriter AddType(Type type);

        IStringWriter AddLine();
        IStringWriter AddSpace();

        IStringWriter AddTabLevel();
        IStringWriter RemoveTabLevel();

        string GetString();
    }
}