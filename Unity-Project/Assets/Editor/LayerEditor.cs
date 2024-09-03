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
    }
}

[CustomEditor(typeof(ContainerMono))]
public class ContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ContainerMono t = (ContainerMono)target;
        DrawDefaultInspector();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            t.SaveToJSON();
        }
        if (GUILayout.Button("Load"))
        {
            t.ReadFromJSON();
        }
        EditorGUILayout.EndHorizontal();
    }
}