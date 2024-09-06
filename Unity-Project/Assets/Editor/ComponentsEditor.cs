using UnityEditor;
using UnityEngine;
using Zenject;

[CustomEditor(typeof(SlotMono))]
public class SlotMonoEditor : Editor
{
    public override void OnInspectorGUI()
    {

        SlotMono t = (SlotMono)target;
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
        EditorGUILayout.Space(5);

        t.ItemId = EditorGUILayout.IntField("Item Id: ", t.ItemId);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (Application.isEditor && !Application.isPlaying )
            GUI.enabled = false;
        if (GUILayout.Button("Load Data"))
        {
            t.LoadData();
        }
        GUI.enabled = true;

        if (GUILayout.Button("Delete Slot"))
        {
            t.RemoveSlot();
        }
        EditorGUILayout.EndHorizontal();
    }
}

[CustomEditor(typeof(LayerMono))]
public class LayerMonoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LayerMono t = (LayerMono)target;

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

        t.LayerOrder = EditorGUILayout.IntField("Layer Order: ", t.LayerOrder);
        t.Status = (LayerStatus)EditorGUILayout.EnumPopup("Layer Status: ", t.Status);
        EditorGUILayout.Space(5);

        EditorGUIUtility.labelWidth = 45.0f;
        for (int i = 0; i < t.MaxSlots; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            t.Slots[i] = EditorGUILayout.ObjectField($"Slot {i}:", t.Slots[i] as MonoBehaviour, typeof(SlotMono), true) as SlotMono;
            if (GUILayout.Button("Add Slot"))
            {
                if (t.Slots[i] == null)
                {
                    t.Slots[i] = PrefabsInstanciatorFactory.InitializeNew(new Slot(), t.transform, false);
                    (t.Slots[i] as MonoBehaviour).transform.name = $"Slot {i}";
                    t.SetStatus(t.Status);
                }

            }
            if (GUILayout.Button("Clear Slot"))
            {
                if (t.Slots[i] != null)
                {
                    t.Slots[i].ClearSlot();
                }
            }
            if (GUILayout.Button("Detach Slot"))
            {
                if (t.Slots[i] != null)
                {
                    (t.Slots[i] as MonoBehaviour).transform.parent = null;
                    t.Slots[i] = null;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(5);

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

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Layer"))
        {
            t.AddLayer();
        }
        if (GUILayout.Button("Rearange Layers"))
        {
            t.RearangeLayers();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Remove Container"))
        {
            t.RemoveContainer();
        }
    }
}
