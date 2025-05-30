using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        SerializedProperty enemies = serializedObject.FindProperty("enemiesToSpawn");

        if (enemies != null && enemies.isArray)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Enemy Probabilities (Must sum 100%)", EditorStyles.boldLabel);

            int total = 0;
            int[] rawValues = new int[enemies.arraySize];

            for (int i = 0; i < enemies.arraySize; i++)
            {
                var element = enemies.GetArrayElementAtIndex(i);
                var prefabProp = element.FindPropertyRelative("prefab");
                var probProp = element.FindPropertyRelative("probability");

                EditorGUILayout.PropertyField(prefabProp);

                int currentValue = Mathf.RoundToInt(probProp.floatValue * 100f);
                rawValues[i] = EditorGUILayout.IntSlider("Probability (%)", currentValue, 0, 100);
                total += rawValues[i];
            }
            if (total != 100 && total > 0)
            {
                float scale = 100f / total;
                int finalSum = 0;

                for (int i = 0; i < enemies.arraySize; i++)
                {
                    int adjusted = Mathf.RoundToInt(rawValues[i] * scale);
                    rawValues[i] = adjusted;
                    finalSum += adjusted;
                }
                int difference = 100 - finalSum;
                rawValues[enemies.arraySize - 1] += difference;
            }
            for (int i = 0; i < enemies.arraySize; i++)
            {
                var probProp = enemies.GetArrayElementAtIndex(i).FindPropertyRelative("probability");
                probProp.floatValue = rawValues[i] / 100f;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}