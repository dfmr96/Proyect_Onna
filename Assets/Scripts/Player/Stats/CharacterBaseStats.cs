using UnityEngine;

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Player Base Stats")]
    public class CharacterBaseStats : ScriptableObject
    {
        [SerializeField] private StatBlock baseStatBlock;
        public StatBlock BaseStats => baseStatBlock;
    }
}
