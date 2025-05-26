using Player.Stats;
using UnityEngine;

namespace Mutations
{
    public abstract class UpgradeEffect : ScriptableObject
    {
        public abstract void Apply(RuntimeStats player);
    }
}