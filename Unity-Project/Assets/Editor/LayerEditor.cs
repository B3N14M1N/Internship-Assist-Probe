using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LayerMono))]
public class LayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LayerMono t = (LayerMono)target;
        DrawDefaultInspector();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Push Layer"))
        {
            t.PushLayer();
        }
        if (GUILayout.Button("Pull Layer"))
        {
            t.PullLayer();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Remove Layer"))
        {
            t.RemoveLayer();
        }
    }
}

[CustomEditor(typeof(ContainerMono))]
public class ContainerMonoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ContainerMono t = (ContainerMono)target;
        DrawDefaultInspector();
        if (GUILayout.Button("AddLayer"))
        {
            t.AddLayer();
        }
        if (GUILayout.Button("Remove Container"))
        {
            t.RemoveContainer();
        }
    }
}

[CustomEditor(typeof(LevelBuilderManager))]
public class LevelBuilderManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelBuilderManager t = (LevelBuilderManager)target;
        DrawDefaultInspector();
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
            t.LoadLevel();
        }
        EditorGUILayout.EndHorizontal();

    }
}