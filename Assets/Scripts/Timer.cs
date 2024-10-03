using UnityEngine;

public class Timer
{
    #region VARIABLES

    private float duration;       // Total duration of the timer
    private float elapsedTime;    // Time elapsed since the timer started
    private bool isRunning;       // Indicates whether the timer is currently running

    #endregion

    #region CONSTRUCTORS

    /// <summary>
    /// Initializes a new Timer instance with the specified duration.
    /// </summary>
    /// <param name="duration">Duration for the timer in seconds.</param>
    public Timer(float duration)
    {
        this.duration = duration;
        this.elapsedTime = 0f;
        this.isRunning = true;
    }

    #endregion

    #region TIMER CONTROL METHODS

    /// <summary>
    /// Resets the timer with a new duration and starts it.
    /// </summary>
    /// <param name="newDuration">The new duration for the timer in seconds.</param>
    public void Reset(float newDuration)
    {
        duration = newDuration;
        elapsedTime = 0f;
        isRunning = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void Stop()
    {
        isRunning = false;
    }

    /// <summary>
    /// Checks if the timer is currently running.
    /// </summary>
    /// <returns>True if the timer is running; otherwise, false.</returns>
    public bool IsRunning()
    {
        return isRunning;
    }

    /// <summary>
    /// Checks if the timer has elapsed its duration.
    /// </summary>
    /// <returns>True if the duration has elapsed; otherwise, false.</returns>
    public bool HasElapsed()
    {
        if (!isRunning) return false;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            elapsedTime = 0f; // Reset the timer for repeated use
            return true;
        }

        return false;
    }

    #endregion
}
