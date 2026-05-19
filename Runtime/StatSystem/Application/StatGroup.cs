
using System.Collections.Generic;
using UnityEngine;

namespace Dave6.StatSystem2.Application
{
    [CreateAssetMenu(fileName = "StatGroup", menuName = "Dave6/StatSystem2/StatGroup")]
    public class StatGroup : ScriptableObject
    {
        public string GroupName;

        public List<StatTag> Tags = new();
    }
}