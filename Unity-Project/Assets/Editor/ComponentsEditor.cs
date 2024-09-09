using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

using GameItemHolders;

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
        ILayer t = (ILayer)target;

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

        EditorGUILayout.Space(5);
        GUI.enabled = false;
        EditorGUILayout.EnumPopup("Layer Status: ", t.Status);
        GUI.enabled = true;
        EditorGUILayout.Space(5);

        EditorGUIUtility.labelWidth = 45.0f;
        for (int i = 0; i < t.MaxSlots; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            var newSlot = EditorGUILayout.ObjectField($"Slot {i}:", t.Slots[i] as MonoBehaviour, typeof(MonoBehaviour), true)?.GetComponent<ISlot>();
            if (newSlot != null && newSlot != t.Slots[i])
            {
                t.Slots[i] = newSlot;
                (t.Slots[i] as MonoBehaviour).transform.parent = (t as MonoBehaviour).transform;
                t.SetStatus(t.Status);
            }
            if (GUILayout.Button("Add Slot"))
            {
                t.Slots[i] ??= PrefabsInstanciatorFactory.InitializeNew(new Slot(), (t as MonoBehaviour).transform, false);
                (t.Slots[i] as MonoBehaviour).transform.name = $"Slot {i}";
            }
            if (GUILayout.Button("Clear Slot"))
            {
                t.Slots[i]?.ClearSlot();
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
        if (GUILayout.Button("Remove Layer"))
        {
            t.RemoveLayer();
        }
        EditorGUILayout.EndHorizontal();
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
            t.RearangeLayers(true);
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Remove Container"))
        {
            t.RemoveContainer();
        }
    }
}
