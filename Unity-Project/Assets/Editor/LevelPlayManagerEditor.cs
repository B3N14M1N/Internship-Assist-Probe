using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelPlayManager))]
public class LevelPlayManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelPlayManager t = (LevelPlayManager)target;
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
        EditorGUILayout.Space(5);

        GUI.enabled = false;
        EditorGUIUtility.labelWidth = 100.0f;
        EditorGUILayout.IntField("Level: ", t.Level);
        EditorGUILayout.FloatField("Starting Time", t.LevelTime);
        GUI.enabled = true;
        EditorGUILayout.Space(5);
        EditorGUIUtility.labelWidth = 0;
        var newComboTime = EditorGUILayout.FloatField("Max Combo Time: ", t.MaxComboTime);
        t.MaxComboTime = newComboTime >= 0 ? newComboTime : 30;

    }
}