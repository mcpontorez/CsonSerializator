using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Wild.Cson.Serialization.Utils
{
    public interface ITypeMemberService
    {
        IReadOnlyList<FieldInfo> GetSerializableMembers(Type type);
    }

    public sealed class TypeMemberService : ITypeMemberService
    {
        private Dictionary<Type, IReadOnlyList<FieldInfo>> _typeMembers = new Dictionary<Type, IReadOnlyList<FieldInfo>>();

        public IReadOnlyList<FieldInfo> GetSerializableMembers(Type type)
        {
            IReadOnlyList<FieldInfo> members = null;
            if (_typeMembers.TryGetValue(type, out members))
                return members;

            members = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsInitOnly).ToArray();
            _typeMembers.Add(type, members);
            return members;
        }
    }
}
