#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mutations
{
    public static class MutationGenerator
    {
        [MenuItem("Tools/Generate Dummy Mutations")]
        public static void GenerateMutations()
        {
            string folderPath = "Assets/Mutations/Generated";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }

            foreach (OrganType organ in System.Enum.GetValues(typeof(OrganType)))
            {
                for (int i = 1; i <= 5; i++)
                {
                    MutationData mutation = ScriptableObject.CreateInstance<MutationData>();

                    string nameBase = $"{organ}Mutation{i}";
                    mutation.MutationName = nameBase;
                    mutation.Description = nameBase;
                    mutation.Organ = organ;

                    string assetName = $"{organ}Test{i}.asset";
                    string assetPath = Path.Combine(folderPath, assetName);

                    AssetDatabase.CreateAsset(mutation, assetPath);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("✅ Dummy mutations generated!");
        }
    }
}
#endif