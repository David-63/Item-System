using Dave6.StatSystem2.Domain;
using UnityEngine;

namespace Dave6.StatSystem2.Application
{
    [CreateAssetMenu(fileName = "StatTag", menuName = "Dave6/StatSystem2/StatTag")]
    public class StatTagAsset : ScriptableObject
    {
        public TagName tagName;
    }
}