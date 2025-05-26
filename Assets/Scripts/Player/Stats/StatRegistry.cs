using System.Collections.Generic;
using UnityEngine;

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Stat Registry", fileName = "StatRegistry")]
    public class StatRegistry : ScriptableObject
    {
        [SerializeField]
        private List<StatDefinition> allStats = new();

        public IReadOnlyList<StatDefinition> AllStats => allStats;

        public StatDefinition GetByName(string statName)
        {
            return allStats.Find(s => s != null && s.name == statName);
        }

        public bool Contains(StatDefinition stat)
        {
            return allStats.Contains(stat);
        }

#if UNITY_EDITOR
        [ContextMenu("Auto-Fill From Project")]
        private void AutoFill()
        {
            allStats.Clear();
            var assets = UnityEditor.AssetDatabase.FindAssets("t:StatDefinition");
            foreach (var guid in assets)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var stat = UnityEditor.AssetDatabase.LoadAssetAtPath<StatDefinition>(path);
                if (stat != null && !allStats.Contains(stat))
                    allStats.Add(stat);
            }

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log($"✅ StatRegistry actualizado con {allStats.Count} stats.");
        }
#endif
    }
}