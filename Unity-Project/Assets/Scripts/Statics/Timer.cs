using System.Collections.Generic;

/// <summary>
/// This class is the timer for the game
/// the timer starts with the set time and needs to be updated
/// each frame (when the game is not paused)
/// This supports multiple timers for multiple purposes and can go in both directions countdown/countup
/// </summary>
public static class Timer
{
    /// <summary>
    /// The timers
    /// </summary>
    private static Dictionary<string, float> kvp = new Dictionary<string, float>();

    /// <summary>
    /// Creates a new timer with the starting time
    /// </summary>
    /// <param name="name"></param>
    /// <param name="time"></param>
    public static void StartTimer(string name, float time)
    {
        if(!kvp.TryAdd(name, time))
            kvp[name] = time;
    }

    /// <summary>
    /// Returns the timer value if it exists, else return 0;
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static float GetTimer(string name)
    {
        if (kvp.TryGetValue(name, out float time))
        {
            return time;
        }
        return 0;
    }

    /// <summary>
    /// Removes the timer if it exists
    /// </summary>
    /// <param name="name"></param>
    public static void RemoveTimer(string name)
    {
        kvp.Remove(name);
    }

    /// <summary>
    /// Can add or substract time from the timer
    /// </summary>
    /// <param name="name"></param>
    /// <param name="timeElapsed"></param>
    public static void UpdateTimer(string name, float timeElapsed)
    {
        if (kvp.ContainsKey(name))
        {
            kvp[name] -= timeElapsed;
        }
    }
}
