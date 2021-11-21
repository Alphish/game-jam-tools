using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamPlayer.Export.Runtime
{
    public readonly struct TypeName : IEquatable<TypeName>
    {
        public CodeName RootName { get; }
        public IReadOnlyCollection<TypeName> GenericArguments { get; }
        
        private string StringName { get; }

        // --------
        // Creation
        // --------
        
        private TypeName(CodeName rootName, IEnumerable<TypeName>? genericArguments = null)
        {
            RootName = rootName;
            GenericArguments = (genericArguments ?? Enumerable.Empty<TypeName>()).ToList();

            StringName = GenericArguments.Any()
                ? $"{RootName}<{string.Join(",", GenericArguments)}>"
                : $"{RootName}";
        }

        public static TypeName CreateSimple(CodeName codeName)
            => new TypeName(codeName);

        public static TypeName CreateSimple(string name)
            => CreateSimple(CodeName.From(name));

        public static TypeName CreateGeneric(CodeName codeName, IEnumerable<TypeName> genericArguments)
            => new TypeName(codeName, genericArguments);
        
        public static TypeName CreateGeneric(CodeName codeName, params TypeName[] genericArguments)
            => CreateGeneric(codeName, genericArguments as IEnumerable<TypeName>);
        
        public static TypeName CreateGeneric(string name, IEnumerable<TypeName> genericArguments)
            => new TypeName(CodeName.From(name), genericArguments);
        
        public static TypeName CreateGeneric(string name, params TypeName[] genericArguments)
            => CreateGeneric(CodeName.From(name), genericArguments as IEnumerable<TypeName>);
        
        // ---------------------
        // String representation
        // ---------------------

        public override string ToString()
        {
            return StringName;
        }

        // ---------------
        // Equality checks
        // ---------------

        public override bool Equals(object? obj)
            => obj is TypeName name && Equals(name);
        public bool Equals(TypeName other)
            => StringComparer.OrdinalIgnoreCase.Equals(StringName, other.StringName);
        public override int GetHashCode()
            => HashCode.Combine(StringComparer.OrdinalIgnoreCase.GetHashCode(StringName));

        public static bool operator ==(TypeName left, TypeName right)
            => left.Equals(right);
        public static bool operator !=(TypeName left, TypeName right)
            => !(left == right);
    }
}