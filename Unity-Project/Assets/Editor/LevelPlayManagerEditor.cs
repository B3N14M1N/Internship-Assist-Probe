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
        EditorGUIUtility.labelWidth = 0;

        EditorGUILayout.Space(5);
        if (Application.isEditor && !Application.isPlaying)
            GUI.enabled = false;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Game"))
        {
            t.StartGame();
        }
        if (GUILayout.Button("Pause Game"))
        {
            t.PauseGame(!t.Paused);
        }
        if (GUILayout.Button("End Game"))
        {
            t.EndGame();
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
    }
}