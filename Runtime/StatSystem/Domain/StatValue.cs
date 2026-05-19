using System.Collections.Generic;

namespace Dave6.StatSystem2.Domain
{
    public class StatValue
    {
        float _BaseValue;
        readonly List<StatModifier> _Modifiers = new();

        public StatValue(float baseValue) => _BaseValue = baseValue;

        public void AddModifier(StatModifier modifier) => _Modifiers.Add(modifier);

        public void RemoveModifier(object source) => _Modifiers.RemoveAll(m => m.Source == source);

        public float Calculate()
        {
            float flat = 0;
            float percent = 0;
            float multiplier = 1f;

            foreach (var modifier in _Modifiers)
            {
                switch (modifier.Type)
                {
                    case ModifierType.Flat:
                        flat += modifier.Value;
                        break;
                    case ModifierType.Percent:
                        percent += modifier.Value;
                        break;
                    case ModifierType.Multiplier:
                        multiplier *= 1 + modifier.Value;
                        break;
                }
            }
            return (_BaseValue + flat) * (1 + percent) * multiplier;
        }
    }
}