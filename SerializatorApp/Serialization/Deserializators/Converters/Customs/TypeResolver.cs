using SerializatorApp.Serialization.Deserializators.Reading;
using SerializatorApp.Serialization.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SerializatorApp.Serialization.Deserializators.Converters.Customs
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
        private Dictionary<string, Type> _cacheGenericCLRNameTypes = new Dictionary<string, Type>();

        public TypeResolver(HashSet<string> usings) => _usings = usings;
        private TypeResolver() => _usings = Array.Empty<string>();

        private bool TryGetFromCacheTypes(string typeName, out Type type) => _cacheTypes.TryGetValue(typeName, out type);
        private bool TryGetFromCacheGenericCLRNameTypes(string typeName, out Type type) => _cacheGenericCLRNameTypes.TryGetValue(typeName, out type);

        private Type GetFromReflection(string typeName)
        {
            Type result = TypeHelper.Get(typeName);
            if (result == null)
            {
                foreach (var item in _usings)
                {
                    result = TypeHelper.Get($"{item}.{typeName}");
                    if (result != null)
                        break;
                }
            }
            return result;
        }

        private Type GetFromCacheTypes(string typeName)
        {
            Type result;
            if (TryGetFromCacheTypes(typeName, out result))
                return result;
            else result = GetFromReflection(typeName);
            if(result != null)
                _cacheTypes.Add(typeName, result);
            return result;
        }

        private Type GetFromCacheGenericCLRNameTypes(string typeName)
        {
            Type result;
            if (TryGetFromCacheGenericCLRNameTypes(typeName, out result))
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
            if (lastChar == CharConsts.EndedSquareBracket || lastChar == CharConsts.EndedAngleBracket || lastChar == CharConsts.BeginedSquareBracket || lastChar == CharConsts.BeginedAngleBracket || lastChar == CharConsts.Comma)
            {
                TypeData typeData = ConvertFromRawName(new CsonReader(typeName));
                type = MakeType(typeData);
            }
            else
                type = GetFromCacheTypes(typeName);

            return type;
        }

        public Type MakeType(TypeData typeData)
        {
            Type type;
            if (typeData.IsGeneric)
            {
                type = GetFromCacheGenericCLRNameTypes(typeData.GetCLRGenericTypeName());
                //TODO: replace linq on non-linq
                Type[] typeParams = typeData.Params.Select(td => MakeType(td)).ToArray();
                type = type.MakeGenericType(typeParams);
            }
            else
                type = GetFromCacheTypes(typeData.TypeName);

            //TODO: set type to cache
            if (typeData.IsArray)
            {
                foreach (var item in typeData.ArrayParams)
                {
                    type = type.MakeArrayType(item.Dimension);
                }
            }
            return type;
        }

        private TypeData ConvertFromRawName(CsonReader cson)
        {
            string typeName = cson.TakeWhile(IsTypeNameChar);
            bool isGeneric = cson.CurrentChar == CharConsts.BeginedAngleBracket;

            TypeData typeData = new TypeData(typeName, isGeneric);

            if (isGeneric)
            {
                do
                {
                    //skip BeginedAngleBracket or Comma
                    cson.SkipOne();
                    typeData.AddParam(ConvertFromRawName(cson));
                } while (cson.CurrentChar == CharConsts.Comma);

                cson.Skip(CharConsts.EndedAngleBracket);
            }

            while (cson.TrySkip(CharConsts.BeginedSquareBracket))
            {
                int dimension = 1;
                while (cson.TrySkip(CharConsts.Comma))
                    dimension++;
                cson.Skip(CharConsts.EndedSquareBracket);
                typeData.AddArrayParam(dimension);
            }

            return typeData;
        }

        private static bool IsTypeNameChar(char value) => char.IsLetterOrDigit(value) || value == CharConsts.Dot || value == '_';
    }

    public sealed class ArrayData
    {
        public static IReadOnlyList<ArrayData> EmptyArrayDatas = new ArrayData[0];

        private readonly static IReadOnlyList<ArrayData> _arrayDatas;

        static ArrayData()
        {
            ArrayData[] arrayDatas = new ArrayData[33];
            for (int i = 1; i < arrayDatas.Length; i++)
            {
                arrayDatas[i] = new ArrayData(i);
            }
            _arrayDatas = arrayDatas;
        }

        public static ArrayData GetInstance(int dimension)
        {
            if (dimension < 1 || dimension > 32)
                throw new ArgumentOutOfRangeException(nameof(dimension), dimension, $"Array {nameof(dimension)} must be minimum 1 and maximum 32");
            return _arrayDatas[dimension];
        }

        public readonly int Dimension = 1;
        private ArrayData(int dimension) => Dimension = dimension;
    }

    public sealed class TypeData
    {
        public readonly string TypeName;

        public readonly bool IsGeneric;

        public bool IsArray { get; private set; }

        //TODO: replace on few fields
        private readonly List<TypeData> _params;
        public IReadOnlyList<TypeData> Params { get; }

        private List<ArrayData> _arrayParams;
        public IReadOnlyCollection<ArrayData> ArrayParams { get; private set; }

        public TypeData(string typeName, bool isGeneric) : this(isGeneric)
        {
            TypeName = typeName;
            IsGeneric = isGeneric;
        }

        private TypeData(bool isGeneric)
        {
            IsGeneric = isGeneric;
            if (IsGeneric)
            {
                _params = new List<TypeData>(2);
                Params = _params;
            }
            else
                Params = Array.Empty<TypeData>();
        }

        public void AddParam(TypeData value) => _params.Add(value);

        public void AddArrayParam(int dimension)
        {
            IsArray = true;
            if (_arrayParams == null)
            {
                //TODO: replace on static array[1] whith 1 or 2 dimensions
                _arrayParams = new List<ArrayData>();
                ArrayParams = _arrayParams;
            }
            else if (dimension > 1)
                throw new ArgumentException("Array of arrays cannot be multidimensional!", nameof(dimension));

            _arrayParams.Add(ArrayData.GetInstance(dimension));
        }

        public string GetCLRGenericTypeName() => $"{TypeName}`{Params.Count}";
    }
}