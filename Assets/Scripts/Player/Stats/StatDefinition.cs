using UnityEngine;

namespace Player.Stats
{
    [CreateAssetMenu(fileName = "New Stat", menuName = "Stats/Definition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        [SerializeField] private string statName;

        public string StatName => statName;
    }
}