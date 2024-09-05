using System.Collections.Generic;
using UnityEngine;

public static class Timer
{
    private static Dictionary<string, float> kvp = new Dictionary<string, float>();

    public static void StartTimer(string name, float time)
    {
        if(!kvp.TryAdd(name, time))
            kvp[name] = time;
    }

    public static float GetTimer(string name)
    {
        if (kvp.TryGetValue(name, out float time))
        {
            return time;
        }
        return 0;
    }
    public static void RemoveTimer(string name)
    {
        kvp.Remove(name);
    }

    public static void UpdateTimer(string name, float timeElapsed)
    {
        if (kvp.ContainsKey(name))
        {
            kvp[name] -= timeElapsed;
        }
    }
}
