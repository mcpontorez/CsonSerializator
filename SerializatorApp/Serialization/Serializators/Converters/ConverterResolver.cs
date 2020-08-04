using System;
using System.Collections.Generic;
using System.Reflection;
using SerializatorApp.Serialization.Serializators.Writing;

namespace SerializatorApp.Serialization.Serializators.Converters
{
    public interface IConverterResolver : IConverter, IConcreteValueConverter
    { }

    public sealed class MainConverterResolver : IConverterResolver
    {
        private readonly IConcreteValueConverterCollection _concreteValueConverterCollection;
        private readonly IConverterCollection _concreteTypeConverterCollection;
        private readonly IConverterCollection _converterCollection;

        public MainConverterResolver()
        {
            _concreteValueConverterCollection = new ConcreteValueConverterCollection(new NullConverter());
            _concreteTypeConverterCollection = new ConcreteTypeConverterCollection(new SingleConverter(), new Int32Converter(), new StringConverter());
            _converterCollection = new ConverterCollection(new CollectionConverter(this), new ObjectConverter(this));
        }

        public void Convert(object source, IStringWriter writer)
        {
            IConverterBase converter = _concreteValueConverterCollection.Get(source);
            if (converter == null)
            {
                TypeInfo type = source.GetType().GetTypeInfo();
                (_concreteTypeConverterCollection.Get(type) ?? _converterCollection.Get(type)).Convert(source, writer);
            }
            else
                converter.Convert(source, writer);
        }

        public bool IsConvertable(TypeInfo type) => _concreteTypeConverterCollection.Contains(type) || _converterCollection.Contains(type);

        public bool IsConvertable(object value) => _concreteValueConverterCollection.Contains(value);
    }
}
