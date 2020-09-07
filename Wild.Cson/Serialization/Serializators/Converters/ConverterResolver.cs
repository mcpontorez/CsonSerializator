using System;
using System.Collections.Generic;
using System.Reflection;
using Wild.Cson.Serialization.Serializators.Writing;
using Wild.Cson.Serialization.Utils;

namespace Wild.Cson.Serialization.Serializators.Converters
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
            _converterCollection = new ConverterCollection(new DictionaryConverter(this), new CollectionConverter(this), new ObjectConverter(this));
        }

        public void Convert(object source, ICsonWriter writer, ITypeMemberService typeMemberService)
        {
            IConverterBase converter = _concreteValueConverterCollection.Get(source);
            if (converter == null)
            {
                Type type = source.GetType();
                converter = _concreteTypeConverterCollection.Get(type);
                if(converter == null)
                    converter = _converterCollection.Get(type);
            }
            converter.Convert(source, writer, typeMemberService);
        }

        public bool IsConvertable(Type type) => _concreteTypeConverterCollection.Contains(type) || _converterCollection.Contains(type);

        public bool IsConvertable(object value) => _concreteValueConverterCollection.Contains(value);
    }
}
