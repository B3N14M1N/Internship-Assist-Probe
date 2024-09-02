using UnityEngine;

public class TimerTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Starting Timer");
            Timer.StartTimer("test");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Time elapsed: " + Timer.GetTimer("test"));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Time elapsed and remove: " + Timer.GetTimer("test"));
            Timer.RemoveTimer("test");
        }
    }
}
