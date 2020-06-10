using System;

namespace SerializatorApp.Serialization.Deserializators.Numerics
{
    public class SingleConverter : IConverter
    {
        private const char _endCharUpperCase = 'F', _endCharLowerCase = 'f';

        public T From<T>(StringReader cson)
        {
            string value = cson.TakeUntil(_endCharUpperCase);
            if(value.Length == 0)
                value = cson.TakeUntil(_endCharLowerCase);
            T result = (T)(object)int.Parse(value);
            cson.SkipUntilSeparator();
            return result;
        }

        public bool IsCanConvertable(StringReader cson)
        {
            int endIndex = cson.IndexOf(_endCharUpperCase);
            if(endIndex < 0)
                endIndex = cson.IndexOf(_endCharLowerCase);
            for (int i = 0; i < endIndex; i++)
            {
                if (!char.IsDigit(cson[i]))
                    return false;
            }
            return true;
        }
    }
}
