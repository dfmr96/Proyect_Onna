using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Block", fileName = "New StatBlock")]
    public class StatBlock : ScriptableObject
    {
        [SerializeField] private List<StatValue> stats = new();
        [SerializeField] private SerializedDictionary<StatDefinition, float> _lookup = new();

        [Header("🆕 New Stat Creation")]
        [SerializeField] private string newStatName = "NewStat";
        [SerializeField] private float newStatValue = 0f;

        public float Get(StatDefinition stat)
        {
            return _lookup.GetValueOrDefault(stat, 0f);
        }

        public void Set(StatDefinition stat, float value)
        {
            _lookup[stat] = value;

            var serialized = stats.Find(s => s.stat == stat);
            if (serialized != null)
                serialized.value = value;
            else
                stats.Add(new StatValue { stat = stat, value = value });
        }

        public IReadOnlyList<StatValue> AllStats => stats;

        [Button("Build Lookup")]
        private void BuildLookup()
        {
            _lookup.Clear();
            foreach (var stat in stats)
            {
                if (stat.stat != null)
                    _lookup[stat.stat] = stat.value;
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif

            Debug.Log($"✅ Lookup construido para {name} con {stats.Count} stats.");
        }

        [Button("Add New Stat")]
        private void AddNewStat()
        {
#if UNITY_EDITOR
            string rootPath = "Assets/Stats";
            string folderPath = $"{rootPath}/Definitions";

            if (!AssetDatabase.IsValidFolder(rootPath))
            {
                AssetDatabase.CreateFolder("Assets", "Stats");
            }

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(rootPath, "Definitions");
            }

            string assetPath = $"{folderPath}/{newStatName}.asset";

            StatDefinition newDef = CreateInstance<StatDefinition>();
            newDef.name = newStatName;

            AssetDatabase.CreateAsset(newDef, assetPath);
            AssetDatabase.SaveAssets();

            var statValue = new StatValue { stat = newDef, value = newStatValue };
            stats.Add(statValue);

            BuildLookup();

            EditorUtility.SetDirty(this);
            Debug.Log($"✅ StatDefinition '{newStatName}' creado y agregado al StatBlock.");
#endif
        }
        
        public void Clear()
        {
            stats.Clear();
            _lookup.Clear();
        }

    }
}
