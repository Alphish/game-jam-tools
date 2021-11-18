using System;
using System.Text.RegularExpressions;

namespace Alphicsh.JamPlayer.Export.Execution
{
    public readonly struct CodeName : IEquatable<CodeName>
    {
        public string Value { get; }

        private CodeName(string name)
        {
            Value = name;
        }

        private static Regex CodeNamePattern { get; } = new Regex(@"^[_A-Za-z][_A-Za-z0-9]*$");

        public static CodeName From(string name)
        {
            if (!CodeNamePattern.IsMatch(name))
            {
                throw new ArgumentException(
                    "The code name must consist only of alphanumeric characters and underscores, and cannot begin with a number.",
                    nameof(name)
                    );
            }

            return new CodeName(name);
        }

        public override string ToString()
        {
            return Value;
        }

        // ---------------
        // Equality checks
        // ---------------

        public override bool Equals(object? obj)
            => obj is CodeName name && Equals(name);
        public bool Equals(CodeName other)
            => StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);
        public override int GetHashCode()
            => HashCode.Combine(StringComparer.OrdinalIgnoreCase.GetHashCode(Value));

        public static bool operator ==(CodeName left, CodeName right)
            => left.Equals(right);
        public static bool operator !=(CodeName left, CodeName right)
            => !(left == right);
    }
}
