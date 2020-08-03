using System;

namespace CeresECL
{
    public struct IntTag
    {
        readonly int value;
        
        IntTag(int value) => this.value = value;

        public static implicit operator IntTag(Enum enumerator) => new IntTag((int)(object)enumerator);
        public static implicit operator IntTag(int value) => new IntTag(value);
        
        public static bool operator ==(IntTag tag, int integer) => tag.value == integer;
        public static bool operator !=(IntTag tag, int integer) => tag.value != integer;
        
        public override string ToString() => value.ToString();

        public string ToStringFull() => "Int Tag value: " + value; // todo add name if it will exist? 
    }
}