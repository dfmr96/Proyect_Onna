using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    SerializedProperty enemies;

    private void OnEnable() { enemies = serializedObject.FindProperty("enemiesToSpawn"); }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        if (enemies != null && enemies.isArray && enemies.arraySize > 0)
        {
            int totalPercent = 0;

            for (int i = 0; i < enemies.arraySize; i++)
            {
                var element = enemies.GetArrayElementAtIndex(i);
                var prefabProp = element.FindPropertyRelative("prefab");
                var percentProp = element.FindPropertyRelative("percentChance");

                EditorGUILayout.PropertyField(prefabProp);

                int oldValue = Mathf.RoundToInt(percentProp.floatValue);
                int newValue = EditorGUILayout.IntSlider("Chance %", oldValue, 0, 100);

                if (newValue != oldValue)
                {
                    percentProp.floatValue = newValue;
                    NormalizePercentages(i);
                }
                totalPercent += Mathf.RoundToInt(percentProp.floatValue);
            }
            EditorGUILayout.LabelField($"Total %: {totalPercent}%", EditorStyles.boldLabel);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void NormalizePercentages(int changedIndex)
    {
        int sumOthers = 0;
        for (int i = 0; i < enemies.arraySize; i++)
        {
            if (i == changedIndex) continue;
            var percentProp = enemies.GetArrayElementAtIndex(i).FindPropertyRelative("percentChance");
            sumOthers += Mathf.RoundToInt(percentProp.floatValue);
        }

        int remaining = 100 - Mathf.RoundToInt(enemies.GetArrayElementAtIndex(changedIndex).FindPropertyRelative("percentChance").floatValue);

        if (remaining < 0) remaining = 0;

        for (int i = 0; i < enemies.arraySize; i++)
        {
            if (i == changedIndex) continue;

            var percentProp = enemies.GetArrayElementAtIndex(i).FindPropertyRelative("percentChance");
            int oldVal = Mathf.RoundToInt(percentProp.floatValue);

            float proportion = (sumOthers > 0) ? (float)oldVal / sumOthers : 1f / (enemies.arraySize - 1);

            int newVal = Mathf.RoundToInt(proportion * remaining);
            percentProp.floatValue = newVal;
        }
        serializedObject.ApplyModifiedProperties();
    }
}