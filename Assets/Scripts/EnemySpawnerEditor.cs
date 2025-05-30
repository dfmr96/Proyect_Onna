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
            EditorGUILayout.LabelField("Enemy Probabilities", EditorStyles.boldLabel);
            for (int i = 0; i < enemies.arraySize; i++)
            {
                var element = enemies.GetArrayElementAtIndex(i);
                var prefabProp = element.FindPropertyRelative("prefab");
                var probProp = element.FindPropertyRelative("probability");

                EditorGUILayout.PropertyField(prefabProp);

                EditorGUI.BeginChangeCheck();
                probProp.floatValue = EditorGUILayout.Slider("Probability", probProp.floatValue, 0f, 1f);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    NormalizeProbabilities(enemies);
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void NormalizeProbabilities(SerializedProperty array)
    {
        float total = 0f;
        for (int i = 0; i < array.arraySize; i++)
        {
            var p = array.GetArrayElementAtIndex(i).FindPropertyRelative("probability").floatValue;
            total += p;
        }
        if (total == 0f) return;

        for (int i = 0; i < array.arraySize; i++)
        {
            var probProp = array.GetArrayElementAtIndex(i).FindPropertyRelative("probability");
            probProp.floatValue = probProp.floatValue / total;
        }
    }
}