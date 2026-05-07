using System.Collections.Generic;
using Dave6.StatSystem2.Domain;

namespace Dave6.StatSystem2.Application
{
    public class StatController
    {
        Dictionary<TagName, Stat> _Stats = new();

        public void Initialize(IEnumerable<TagName> tags)
        {
            foreach (var tag in tags)
            {
                _Stats[tag] = new Stat(0);
            }
        }
        public float GetValue(TagName tag)
        {
            return _Stats.TryGetValue(tag, out var stat) ? stat.Calculate() : 0f;
        }

        public void ApplyModifier(StatModifier modifier)
        {
            if (!_Stats.TryGetValue(modifier.Tag, out var stat)) return;

            stat.AddModifier(modifier);
        }

        public void RemoveSource(object item)
        {
            foreach (var stat in _Stats.Values)
            {
                stat.RemoveModifier(item);
            }
        }
    }
}