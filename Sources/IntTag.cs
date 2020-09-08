using System;

namespace CeresECL
{
    public readonly struct IntTag
    {
        readonly int value;
        
        IntTag(int value) => this.value = value;

        public static implicit operator IntTag(Enum enumerator) => new IntTag((int)(object)enumerator);
        public static implicit operator IntTag(int value) => new IntTag(value);
        
        public static explicit operator int(IntTag intTag) => intTag.value;
        
        public static bool operator ==(IntTag tag, int integer) => tag.value == integer;
        public static bool operator !=(IntTag tag, int integer) => tag.value != integer;
        
        public override string ToString() => value.ToString();
        
        public bool Equals(IntTag other) => value == other.value;
        public override bool Equals(object tag) => tag is IntTag other && Equals(other);
        public override int GetHashCode() => value;
    }
}