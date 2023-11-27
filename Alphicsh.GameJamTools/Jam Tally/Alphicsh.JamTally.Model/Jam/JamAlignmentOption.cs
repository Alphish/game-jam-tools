using System;
using System.Collections.Generic;

namespace Alphicsh.JamTally.Model.Jam
{
    public class JamAlignmentOption : IEquatable<JamAlignmentOption?>
    {
        public string Title { get; init; } = default!;
        public string ShortTitle { get; init; } = default!;

        public override string ToString() => Title;

        // -------------------
        // Equality comparison
        // -------------------

        public override bool Equals(object? obj)
        {
            return Equals(obj as JamAlignmentOption);
        }

        public bool Equals(JamAlignmentOption? other)
        {
            return other is not null
                && Title == other.Title
                && ShortTitle == other.ShortTitle;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, ShortTitle);
        }

        public static bool operator ==(JamAlignmentOption? left, JamAlignmentOption? right)
        {
            return EqualityComparer<JamAlignmentOption>.Default.Equals(left, right);
        }

        public static bool operator !=(JamAlignmentOption? left, JamAlignmentOption? right)
        {
            return !(left == right);
        }
    }
}
