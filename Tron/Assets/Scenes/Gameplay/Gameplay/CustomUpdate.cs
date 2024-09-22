using System;
using UnityEngine;

public class CustomFixedTimer : MonoBehaviour
{
    private float intervalInSeconds = 1f; // Default interval in seconds
    private float accumulatedTime = 0f; // Accumulates time to track the interval

    // Action that stores the function to be executed at each interval
    private Action intervalFunction;

    // Initializes the timer with a specific interval
    public void Initialize(float interval)
    {
        intervalInSeconds = interval;
    }

    // Updates the interval dynamically
    public void SetInterval(float newInterval)
    {
        intervalInSeconds = newInterval;
    }

    // Set the function to be executed at the interval
    public void SetFunction(Action function)
    {
        intervalFunction = function;
    }

    // This works exactly like Unity's FixedUpdate
    void FixedUpdate()
    {
        // Accumulate time with the fixedDeltaTime
        accumulatedTime += Time.fixedDeltaTime;

        // If the accumulated time reaches the interval, execute the function
        if (accumulatedTime >= intervalInSeconds)
        {
            intervalFunction?.Invoke(); // Invoke the function if it's set
            accumulatedTime = 0f; // Reset the accumulated time after execution
        }
    }
}
