using Wild.Cson.Serialization.Deserializators.Converters.Customs;
using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public class MainConverter
    {
        private readonly UsingConverter _usingConverter = new UsingConverter();
        private readonly IConverterResolver _converterResolver = new MainConverterResolver();

        public TResult Convert<TResult>(string cson)
        {
            CsonReader csonReader = new CsonReader(cson);
            csonReader.SkipWhileSeparators();

            ITypeResolver typeResolver;
            if (_usingConverter.IsCanConvertable(csonReader))
                typeResolver = new TypeResolver(_usingConverter.Convert(csonReader));
            else typeResolver = TypeResolver.InstanceWhithoutUsings;

            ITypeMemberService typeMemberService = new TypeMemberService();

            TResult result = _converterResolver.Convert<TResult>(csonReader, typeResolver, typeMemberService);

            csonReader.SkipWhileSeparators().Skip(CharConsts.Semicolon);

            return result;
        }
    }
}
