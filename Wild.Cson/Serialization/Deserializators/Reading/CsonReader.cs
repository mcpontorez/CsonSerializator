using Wild.Cson.Serialization.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wild.Cson.Serialization.Deserializators.Reading
{
    public class CsonReader
    {
        public int Index { get; private set; } = 0;
        public readonly string Target = null;

        public char this[int index] => Target[Index + index];

        public int Lenght => Target.Length - Index;

        public int GetTrueLenght(int preferLenght) => preferLenght < Lenght ? preferLenght : Lenght;

        public char CurrentChar => this[0];

        public bool IsNotEnded => Lenght > 0;

        public string Buffer => Substring(0);
        public CsonReader(string target)
        {
            Target = target;
        }

        public CsonReader(string target, int index)
        {
            Target = target;
            Index = index;
        }

        public void AddIndex(int value) => Index += value;
        public void AddIndex() => AddIndex(1);
        public bool TryAddIndex(int value)
        {
            if (Lenght > value)
            {
                AddIndex(value);
                return true;
            }
            return false;
        }

        public void TryAddIndex() => TryAddIndex(1);

        public string Substring(int index) => Substring(index, Lenght);
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

        public string TakeWhile(char value) => TakeWhile(c => c == value);

        public string TakeUntil(Func<char, bool> predicate) => TakeWhile(c => !predicate(c));
        public string TakeUntil(char c)
        {
            int startIndex = Index, endIndex = startIndex;
            for (; endIndex < Target.Length; endIndex++)
            {
                char @char = Target[endIndex];
                if (@char == c)
                    break;
            }
            Index = endIndex;
            return Target.Substring(startIndex, endIndex - startIndex);
        }
        public string TakeUntilSeparatorsOr(char value)
        {
            int startIndex = Index, endIndex = startIndex;
            for (; endIndex < Target.Length; endIndex++)
            {
                char @char = Target[endIndex];
                if (@char == value || @char.IsSeparator())
                    break;
            }
            Index = endIndex;
            return Target.Substring(startIndex, endIndex - startIndex);
        }

        public CsonReader Skip(int count)
        {
            Index += count;
            return this;
        }        

        public CsonReader SkipOne() => Skip(1);

        public CsonReader Skip(char value)
        {
            if (StartsWith(value))
                SkipOne();
            else
                throw new Exception($"{value} expected!");
            return this;
        }

        public CsonReader SkipIfNeeds(char value)
        {
            if (StartsWith(value))
                SkipOne();
            return this;
        }

        public bool TrySkip(char value)
        {
            bool result = IsNotEnded && CurrentChar == value;
            if (result)
                SkipOne();
            return result;
        }

        public CsonReader SkipAnyIfNeeds(IReadOnlyList<char> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (StartsWith(values[i]))
                {
                    SkipOne();
                    break;
                }
            }
            return this;
        }

        public CsonReader SkipWhileSeparators()
        {
            int endIndex = Index;
            for (; endIndex < Target.Length; endIndex++)
            {
                char @char = Target[endIndex];
                if (!@char.IsSeparator())
                    break;
            }
            Index = endIndex;
            return this;
        }

        public CsonReader SkipUntil(Func<char, bool> predicate) => SkipWhile(c => !predicate(c));

        public CsonReader SkipWhile(Func<char, bool> predicate)
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

        public CsonReader SkipStartsWith(string value) => Skip(value.Length);

        public bool StartsWith(string value) => IndexOf(value) == 0;

        public bool StartsWith(char value) => IsNotEnded && CurrentChar == value;

        public int StartsCount(Func<char, bool> predicate, int startIndex, int count)
        {
            int lenght = (count <= Lenght) ? count : Lenght;
            int result = 0;
            for (int i = startIndex; i < lenght; i++)
            {
                char c = this[i];
                if (!predicate(c))
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        public int IndexOf(char value) => IndexOf(value, 0);
        public int IndexOf(char value, int startIndex) => Target.IndexOf(value, Index + startIndex) - Index;
        public int IndexOf(char value, int startIndex, int count) => 
            Target.IndexOf(value, Index + startIndex, GetTrueLenght(count)) - Index;
        public int IndexOfAny(char[] values, int startIndex, int count) => 
            Target.IndexOfAny(values, Index + startIndex, GetTrueLenght(count)) - Index;

        public int IndexOf(string value) => IndexOf(value, 0);
        public int IndexOf(string value, int startIndex) => Target.IndexOf(value, Index + startIndex, StringComparison.Ordinal) - Index;

        public override string ToString() => Buffer;
    }
}
