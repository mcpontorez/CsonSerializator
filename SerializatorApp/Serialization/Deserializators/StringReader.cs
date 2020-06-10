using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators
{
    public class StringReader
    {
        public int Index = 0;
        public readonly string Target = null;

        public char this[int index] => Target[Index + index];

        public int Lenght => Target.Length - Index;
        public StringReader(string target)
        {
            Target = target;
        }

        public StringReader(string target, int index)
        {
            Target = target;
            Index = index;
        }

        public string Take(int count)
        {
            string result = Target.Substring(Index, count);
            Index += count;
            return result;
        }

        public string TakeWhile(Func<char, bool> predicate)
        {
            int startIndex = Index, endIndex = startIndex;
            for (; endIndex < Target.Length; endIndex++)
            {
                char @char = Target[endIndex];
                if (!predicate(@char))
                    break;
            }
            Index = endIndex;
            return Target.Substring(startIndex, endIndex - startIndex);
        }

        public string TakeUntil(char value) => TakeUntil(c => c != value);
        public string TakeUntil(Func<char, bool> predicate)
        {
            int startIndex = Index, endIndex = startIndex;
            for (; endIndex < Target.Length; endIndex++)
            {
                char @char = Target[endIndex];
                if (!predicate(@char))
                    break;
            }
            Index = endIndex;
            return Target.Substring(startIndex, endIndex - startIndex);
        }

        public StringReader Skip(int count)
        {
            Index += count;
            return this;
        }

        public StringReader SkipOne() => Skip(1);

        public StringReader SkipSeparators()
        {
            int i = Index;
            for (; i < Target.Length; i++)
            {
                if (!(char.IsSeparator(Target, i) || char.IsControl(Target, i)))
                    break;
            }
            Index = i;
            return this;
        }

        public StringReader SkipUntilSeparator() => SkipUntil(c => char.IsSeparator(c));

        public StringReader SkipUntil(Func<char, bool> predicate) => SkipWhile(c => !predicate(c));

        public StringReader SkipWhile(Func<char, bool> predicate)
        {
            int endIndex = Index;
            for (; endIndex < Target.Length; endIndex++)
            {
                char @char = Target[endIndex];
                if (!predicate(@char))
                    break;
            }
            Index = endIndex;
            return this;
        }

        public bool TrySkipStartsWith(string value)
        {
            if (StartsWith(value))
            {
                Skip(value.Length);
                return true;
            }
            return false;  
        }

        public void SkipStartsWith(string value) => Skip(value.Length);

        public char GetCurrentChar() => Target[Index];

        public bool StartsWith(string value) => IndexOf(value) == 0;
        public bool StartsWith(char value) => IndexOf(value) == 0;

        public int IndexOf(char value) => Target.IndexOf(value, Index);
        public int IndexOf(char value, int startIndex) => Target.IndexOf(value, Index + startIndex);

        public int IndexOf(string value) => Target.IndexOf(value, Index);
        public int IndexOf(string value, int startIndex) => Target.IndexOf(value, Index + startIndex);

        //public StringBuilder Get(int lenght) => new StringBuilder()
    }
}
