using System;
using System.Collections.Generic;

namespace Wild.Cson.Serialization.Deserializators.Reading
{
    public interface ICsonReader
    {
        char this[int index] { get; }

        string Buffer { get; }
        char CurrentChar { get; }
        int Index { get; }
        bool IsNotEnded { get; }
        int Lenght { get; }

        void AddIndex();
        void AddIndex(int value);
        int GetTrueLenght(int preferLenght);
        int IndexOf(char value);
        int IndexOf(char value, int startIndex);
        int IndexOf(char value, int startIndex, int count);
        int IndexOf(string value);
        int IndexOf(string value, int startIndex);
        int IndexOfAny(char[] values, int startIndex, int count);
        ICsonReader Skip(char value);
        ICsonReader Skip(int count);
        ICsonReader SkipAnyIfNeeds(IReadOnlyList<char> values);
        ICsonReader SkipIfNeeds(char value);
        ICsonReader SkipOne();
        ICsonReader SkipStartsWith(string value);
        ICsonReader SkipUntil(Func<char, bool> predicate);
        ICsonReader SkipWhile(Func<char, bool> predicate);
        ICsonReader SkipWhileSeparators();
        int StartsCount(Func<char, bool> predicate, int startIndex, int count);
        bool StartsWith(char value);
        bool StartsWith(string value);
        string Substring(int index);
        string Substring(int index, int count);
        string Take(int count);
        string TakeUntil(char c);
        string TakeUntil(Func<char, bool> predicate);
        string TakeUntilSeparatorsOr(char value);
        string TakeWhile(char value);
        string TakeWhile(Func<char, bool> predicate);
        string ToString();
        void TryAddIndex();
        bool TryAddIndex(int value);
        bool TrySkip(char value);
        bool TrySkipStartsWith(string value);
    }
}