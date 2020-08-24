using System;

namespace Wild.Cson.Serialization
{
    public static class CsonUtil
    {
        private static readonly Serializators.Converters.MainConverter Serializator = new Serializators.Converters.MainConverter();
        private static readonly Deserializators.Converters.MainConverter Deserializator = new Deserializators.Converters.MainConverter();

        public static string To<T>(T source)
        {
            string cson = Serializator.Convert(source);
            return cson;
        }

        public static T From<T>(string cson)
        {
            T result = Deserializator.Convert<T>(cson);
            return result;
        }
    }
}
