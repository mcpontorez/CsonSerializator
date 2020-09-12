using Wild.Cson.Serialization.Deserializators.Reading;
using Wild.Cson.Serialization.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Wild.Cson.Serialization.Deserializators.Converters
{
    public interface ITypeResolver
    {
        Type Convert(string typeName);
    }

    public sealed class TypeResolver : ITypeResolver
    {
        public static readonly TypeResolver InstanceWhithoutUsings = new TypeResolver();

        private readonly IEnumerable<string> _usings;
        //TODO: different cache for non-generic and generic types and array
        private Dictionary<string, Type> _cacheTypes = new Dictionary<string, Type>();
        private Dictionary<string, Type> _cacheSuperTypes = new Dictionary<string, Type>();
        private Dictionary<string, Type> _cacheGenericCLRNameTypes = new Dictionary<string, Type>();

        public TypeResolver(HashSet<string> usings) => _usings = usings;
        private TypeResolver() => _usings = Array.Empty<string>();

        private Type GetFromReflection(string typeName)
        {
            Type result = TypeHelper.Get(typeName);
            if (result == null)
                result = TypeHelper.Get(_usings, typeName);
            return result;
        }

        private Type GetFromCacheTypes(string typeName)
        {
            Type result;
            if (_cacheTypes.TryGetValue(typeName, out result))
                return result;
            else result = GetFromReflection(typeName);
            if(result != null)
                _cacheTypes.Add(typeName, result);
            return result;
        }

        private Type GetFromCacheGenericCLRNameTypes(string typeName)
        {
            Type result;
            if (_cacheGenericCLRNameTypes.TryGetValue(typeName, out result))
                return result;
            else result = GetFromReflection(typeName);
            if (result != null)
                _cacheGenericCLRNameTypes.Add(typeName, result);
            return result;
        }

        public Type Convert(string typeName)
        {
            typeName = typeName.RemoveSeparators();
            Type type = null;

            char lastChar = typeName[typeName.Length - 1];
            if (lastChar == CharConsts.EndedSquareBracket || lastChar == CharConsts.EndedAngleBracket)
            {
                if (!_cacheSuperTypes.TryGetValue(typeName, out type))
                {
                    TypeData typeData = ConvertFromRawName(new CsonReader(typeName));
                    type = MakeType(typeData);
                }
            }
            else
                type = GetFromCacheTypes(typeName);

            return type;
        }

        private TypeData ConvertFromRawName(CsonReader cson)
        {
            int typeNameStartIndex = cson.Index;
            string typeName = cson.TakeWhile(IsTypeNameChar);
            bool isGeneric = cson.TrySkip(CharConsts.BeginedAngleBracket), isArray;

            IReadOnlyList<TypeData> resultTypeParams;
            if (isGeneric)
            {
                List<TypeData> typeParams = new List<TypeData>(2);
                do
                {
                    typeParams.Add(ConvertFromRawName(cson));
                } while (cson.TrySkip(CharConsts.Comma));

                cson.Skip(CharConsts.EndedAngleBracket);

                resultTypeParams = typeParams;
            }
            else
                resultTypeParams = Array.Empty<TypeData>();

            ArrayParam arrayParam = new ArrayParam();
            if (isArray = cson.StartsWith(CharConsts.BeginedSquareBracket))
            {
                bool index = false;
                while (cson.TrySkip(CharConsts.BeginedSquareBracket))
                {
                    int dimension = 1;
                    while (cson.TrySkip(CharConsts.Comma))
                        dimension++;
                    cson.Skip(CharConsts.EndedSquareBracket);

                    if(!index)
                    {
                        index = true;
                        arrayParam = new ArrayParam(dimension);
                    }
                    else
                    {
                        if (dimension > 1)
                            throw new ArgumentException("Array of arrays cannot be multidimensional!", nameof(dimension));
                        arrayParam.ArrayOfArrayCount++;
                    }
                }
            }
            string typeFullName = cson.Index - typeNameStartIndex > typeName.Length ? cson.Target.Substring(typeNameStartIndex, cson.Index) : typeName;
            TypeData typeData = new TypeData(typeName, typeFullName, isGeneric, resultTypeParams, isArray, arrayParam);
            return typeData;
        }

        private Type MakeType(TypeData typeData)
        {
            Type type;
            if (_cacheSuperTypes.TryGetValue(typeData.TypeFullName, out type))
                return type;

            if (typeData.IsGeneric)
            {
                type = GetFromCacheGenericCLRNameTypes(typeData.GetCLRGenericTypeName());
                //TODO: rewrite on non linq
                Type[] typeParams = typeData.TypeParams.Select(td => MakeType(td)).ToArray();
                type = type.MakeGenericType(typeParams);
            }
            else
                type = GetFromCacheTypes(typeData.TypeName);

            if (typeData.IsArray)
            {
                ArrayParam arrayParams = typeData.ArrayParams;
                type = type.MakeArrayType(arrayParams.Dimension);
                for (int i = 0; i < arrayParams.ArrayOfArrayCount; i++)
                {
                    type = type.MakeArrayType();
                }
            }

            _cacheSuperTypes.Add(typeData.TypeFullName, type);
            return type;
        }

        private static bool IsTypeNameChar(char value) => char.IsLetterOrDigit(value) || value == CharConsts.Dot || value == '_';

        private struct ArrayParam
        {
            public readonly int Dimension;
            public int ArrayOfArrayCount;
            public ArrayParam(int dimension)
            {
                Dimension = dimension;
                ArrayOfArrayCount = 0;
            }
        }

        private sealed class TypeData
        {
            public readonly string TypeName, TypeFullName;

            public readonly bool IsGeneric;

            public bool IsArray { get; private set; }

            public readonly IReadOnlyList<TypeData> TypeParams;

            public ArrayParam ArrayParams { get; private set; }

            public TypeData(string typeName, string typeFullName, bool isGeneric, IReadOnlyList<TypeData> typeParams, bool isArray, ArrayParam arrayParam)
            {
                TypeName = typeName;
                TypeFullName = typeFullName;
                IsGeneric = isGeneric;
                TypeParams = typeParams;
                IsArray = isArray;
                ArrayParams = arrayParam;
            }

            public string GetCLRGenericTypeName() => $"{TypeName}`{TypeParams.Count}";
        }
    }
}