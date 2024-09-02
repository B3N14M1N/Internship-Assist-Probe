using System.Collections.Generic;
using UnityEngine;

public static class Timer
{
    private static Dictionary<string, float> kvp = new Dictionary<string, float>();


    public static void StartTimer(string name)
    {
        if(!kvp.TryAdd(name, Time.time))
            kvp[name] = Time.time;
    }

    public static float GetTimer(string name)
    {
        if (!kvp.ContainsKey(name))
            StartTimer(name);
        return Time.time - kvp[name];
    }
    public static void RemoveTimer(string name)
    {
        kvp.Remove(name);
    }

}
