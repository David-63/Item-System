using System;
using System.Collections.Generic;
using Dave6.StatSystem2.Domain;

namespace Dave6.StatSystem2.Application
{
    public class StatController
    {
        Dictionary<StatTag, StatValue> _Stats = new();

        public event Action<StatTag, float> OnStatChanged;

        public void Initialize(IEnumerable<StatTag> tags)
        {
            foreach (var tag in tags)
            {
                _Stats[tag] = new StatValue(0);
            }
        }
        public void Initialize(IEnumerable<StatGroup> groups)
        {
            foreach (var group in groups)
            {
                foreach (var tag in group.Tags)
                {
                    _Stats[tag] = new StatValue(0);
                }
            }
        }
        public float GetValue(StatTag tag)
        {
            return _Stats.TryGetValue(tag, out var stat) ? stat.Calculate() : 0f;
        }
        public bool TryGetStatValue(StatTag tag, out StatValue stat)
        {
            return _Stats.TryGetValue(tag, out stat);
        }

        public void ApplyModifier(StatModifier modifier)
        {
            if (!_Stats.TryGetValue(modifier.Tag, out var stat)) return;

            stat.AddModifier(modifier);
            OnStatChanged?.Invoke(modifier.Tag, stat.Calculate());
        }

        public void RemoveSource(object item)
        {
            foreach (var pair in _Stats)
            {
                pair.Value.RemoveModifier(item);
                OnStatChanged?.Invoke(pair.Key, pair.Value.Calculate());
            }
        }
    }
}