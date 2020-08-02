using System;
using System.Collections.Generic;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public interface IConverterResolver : IConverter
    { }
    public class ConverterResolver : IConverterResolver
    {
        protected readonly IConverterCollection _converterCollection;

        public ConverterResolver(params IConverter[] converters) => _converterCollection = new ConverterCollection(converters);

        public virtual void Convert(object source, IStringWriter writer) => _converterCollection.Get(source.GetType());

        public virtual bool IsCanConvertable(Type type) => _converterCollection.Contains(type);
    }

    public sealed class MainConverterResolver : IConverterResolver
    {
        private readonly IConverterCollection _concreteConverterCollection;
        private readonly IConverterCollection _converterCollection;

        public MainConverterResolver()
        {
            _concreteConverterCollection = new ConcreteConverterCollection(new SingleConverter(), new Int32Converter(), new StringConverter());
            _converterCollection = new ConverterCollection(new ObjectConverter(this));
        }

        public void Convert(object source, IStringWriter writer)
        {
            Type type = source?.GetType() ?? typeof(object);
            (_concreteConverterCollection.Get(type) ?? _converterCollection.Get(type)).Convert(source, writer);
        }

        public bool IsCanConvertable(Type type) => _concreteConverterCollection.Contains(type) || _converterCollection.Contains(type);
    }
}
