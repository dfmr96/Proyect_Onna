using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Player.Stats.Interfaces;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Block", fileName = "New StatBlock")]
    public class StatBlock : ScriptableObject, IStatContainer
    {
        [SerializeField] private StatContainerLogic container = new();

        [Header("🆕 New Stat Creation")] [SerializeField]
        private string newStatName = "NewStat";

        [SerializeField] private float newStatValue = 0f;

        public float Get(StatDefinition stat) => container.Get(stat);
        public void Set(StatDefinition stat, float value) => container.Set(stat, value);
        public IReadOnlyDictionary<StatDefinition, float> All => container.All;
        public void Clear() => container.Clear();

        [Button("Build Lookup")]
        private void RebuildLookup() => container.RebuildLookup();

        [Button("Add New Stat")]
        private void AddNewStat()
        {
#if UNITY_EDITOR
            string path = "Assets/Stats/Definitions";
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder("Assets/Stats", "Definitions");

            var def = CreateInstance<StatDefinition>();
            def.name = newStatName;
            AssetDatabase.CreateAsset(def, $"{path}/{newStatName}.asset");
            AssetDatabase.SaveAssets();

            Set(def, newStatValue);
            EditorUtility.SetDirty(this);
#endif
        }

#if UNITY_EDITOR
        [Button("Merge From Registry")]
        private void MergeFromRegistry()
        {
            string assetPath = "Assets/Stats/StatRegistry.asset";
            StatRegistry registry = AssetDatabase.LoadAssetAtPath<StatRegistry>(assetPath);

            if (registry == null)
            {
                Debug.LogWarning($"❌ No se encontró StatRegistry en {assetPath}");
                return;
            }

            int added = 0;

            foreach (var def in registry.AllStats)
            {
                if (def == null)
                    continue;

                if (!container.All.ContainsKey(def))
                {
                    container.Set(def, 0f);
                    added++;
                }
            }

            container.RebuildLookup();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            Debug.Log($"✅ Merge completo. Se añadieron {added} nuevos stats desde StatRegistry a {name}.");
        }
#endif
    }
}
