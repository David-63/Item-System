using System;
using Dave6.StatSystem2.Application;

namespace Dave6.StatSystem2.Domain
{
    [Serializable]
    public struct StatModifier
    {
        public StatTag Tag;
        public ModifierType Type;
        public float Value;
        public object Source;

        public StatModifier(object source, StatTag tag, ModifierType type, float value)
        {
            Source = source;
            Tag = tag;
            Type = type;
            Value = value;
        }
    }
}