using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Wild.Cson.Serialization.Utils;
using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Deserializators.Utils;

namespace Wild.Cson.Serialization.Deserializators.Converters.Customs
{
    public class ObjectConverter : ICustomTypeConverter
    {
        private readonly IConverterResolver _mainConverterResolver;

        public ObjectConverter(IConverterResolver mainConverterResolver) => _mainConverterResolver = mainConverterResolver;

        public TResult Convert<TResult>(Type type, CsonReader cson, ITypeResolver typeResolver, ITypeMemberService typeMemberService)
        {
            IFieldInfoDictionary resultFieldInfos = typeMemberService.GetSerializableMemberDictionary(type);

            object resultValue = Activator.CreateInstance(type);

            cson.Skip(CharConsts.BeginedBrace);

            bool index = false;
            while (!cson.SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
            {
                if (index && cson.Skip(CharConsts.Comma).SkipWhileSeparators().TrySkip(CharConsts.EndedBrace))
                    break;
                else
                    index = true;

                cson.SkipIfNeeds(CharConsts.AtSign);
                string memberName = cson.TakeWhile(IsMemberNameChar);
                FieldInfo fieldInfo = resultFieldInfos.GetAndRemove(memberName);

                cson.SkipWhileSeparators().Skip(CharConsts.Equal).SkipWhileSeparators();
                object fieldValue = _mainConverterResolver.Convert<object>(cson, typeResolver, typeMemberService);
                fieldInfo.SetValue(resultValue, fieldValue);
            }

            return (TResult)resultValue;
        }

        public bool IsConvertable(Type type) => true;

        private static bool IsMemberNameChar(char value) => char.IsLetterOrDigit(value) || value == CharConsts.Underscore;
    }
}
