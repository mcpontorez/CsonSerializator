using System;
using System.Collections.Generic;

namespace SerializatorApp.Serialization.Models
{
    public class CsonItem : CsonItemBase
    {
        public Dictionary<string, CsonItemBase> Items { get; }
        public CsonItem(Type type, Dictionary<string, CsonItemBase> items) : base(type)
        {
            Items = items;
        }
    }
}
