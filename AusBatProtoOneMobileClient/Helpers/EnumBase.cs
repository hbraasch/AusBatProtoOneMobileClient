using AusBatProtoOneMobileClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AusBatProtoOneMobileClient.Helpers
{
    public abstract class EnumBaseType
    {
        protected static List<T> enumValues = new List<T>();

        public readonly int Key;
        public readonly string Value;
        public readonly string Prompt;

        public EnumBaseType(int key, string value, string prompt)
        {
            Key = key;
            Value = value;
            Prompt = prompt;
            enumValues.Add((T)this);
        }

        protected static ReadOnlyCollection<T> GetBaseValues()
        {
            return enumValues.AsReadOnly();
        }

        protected static T GetBaseByKey(int key)
        {
            foreach (T t in enumValues)
            {
                if (t.Key == key) return t;
            }
            return null;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}



