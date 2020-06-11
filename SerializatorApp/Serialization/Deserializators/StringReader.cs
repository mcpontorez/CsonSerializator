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

        public char CurrentChar => this[0];

        public string Buffer => Substring(0);
        public StringReader(string target)
        {
            Target = target;
        }

        public StringReader(string target, int index)
        {
            Target = target;
            Index = index;
        }

        public string Substring(int index) => Substring(0, Lenght);
        public string Substring(int index, int count) => Target.Substring(Index + index, count);

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

        public StringReader SkipWhileSeparators() => SkipWhile(c => char.IsSeparator(c) || char.IsControl(c));

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

        public bool StartsWith(string value) => IndexOf(value) == 0;

        public bool StartsWith(char value) => IndexOf(value) == 0;

        public int IndexOf(char value) => IndexOf(value, 0);
        public int IndexOf(char value, int startIndex) => Target.IndexOf(value, Index + startIndex) - Index;

        public int IndexOf(string value) => IndexOf(value, 0);
        public int IndexOf(string value, int startIndex) => Target.IndexOf(value, Index + startIndex) - Index;

        public override string ToString() => Buffer;
    }
}
