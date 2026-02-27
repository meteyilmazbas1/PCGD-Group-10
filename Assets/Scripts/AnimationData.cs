using System.Collections.Generic;
using UnityEngine;

namespace UrbanNinja
{
    [CreateAssetMenu(menuName = "Urban Ninja/Animation data")]
    public class AnimationData: ScriptableObject
    {
        [SerializeField] List<string> _punches;
        [SerializeField] List<string> _kicks;

        
        public IEnumerable<string> Punches => _punches;
        public IEnumerable<string> Kicks => _kicks;

    }

}