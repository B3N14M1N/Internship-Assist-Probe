using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelBuilderManager))]
public class LevelBuilderManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelBuilderManager t = (LevelBuilderManager)target;
        DrawDefaultInspector();
        EditorGUILayout.Space(5);
        if (GUILayout.Button("Add Container"))
        {
            t.AddContainer();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            t.SaveLevel();
        }
        if (GUILayout.Button("Reset"))
        {
            t.ResetLevel();
        }
        if (GUILayout.Button("Load"))
        {
            t.LoadLevel(t.Level);
        }
        EditorGUILayout.EndHorizontal();

    }
}