using UnityEngine;

public class Timer
{
    #region VARIABLES

    private float duration;
    private float elapsedTime;
    private bool isRunning;
    private readonly bool autoReset;
    private float timeScale = 1.0f; // Allows for custom time scaling

    public System.Action OnComplete;  // Event to be called when the timer finishes

    #endregion

    #region CONSTRUCTORS

    /// <summary>
    /// Initializes a new Timer instance with the specified duration.
    /// </summary>
    /// <param name="duration">Duration for the timer in seconds.</param>
    /// <param name="autoReset">Indicates if the timer should automatically reset after elapsing.</param>
    public Timer(float duration, bool autoReset = false)
    {
        this.duration = duration;
        this.autoReset = autoReset;
        Reset();
    }

    #endregion

    #region TIMER CONTROL METHODS

    /// <summary>
    /// Starts or restarts the timer.
    /// </summary>
    public void Start()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    /// <summary>
    /// Resets the timer with an optional new duration and starts it.
    /// </summary>
    /// <param name="newDuration">Optional new duration for the timer in seconds.</param>
    public void Reset(float? newDuration = null)
    {
        duration = newDuration ?? duration;
        elapsedTime = 0f;
        isRunning = true;
    }

    /// <summary>
    /// Stops the timer and resets the elapsed time to 0.
    /// </summary>
    public void Stop()
    {
        isRunning = false;
        elapsedTime = 0f;
    }

    /// <summary>
    /// Pauses the timer without resetting the elapsed time.
    /// </summary>
    public void Pause() => isRunning = false;

    /// <summary>
    /// Resumes a paused timer.
    /// </summary>
    public void Resume() => isRunning = true;

    /// <summary>
    /// Toggles between pause and resume states.
    /// </summary>
    public void TogglePause() => isRunning = !isRunning;

    /// <summary>
    /// Checks if the timer is currently running.
    /// </summary>
    /// <returns>True if the timer is running; otherwise, false.</returns>
    public bool IsRunning() => isRunning;

    /// <summary>
    /// Sets a custom time scale for the timer. Adjusts how fast or slow the timer counts down.
    /// </summary>
    /// <param name="scale">Multiplier for the time scale (default is 1.0f).</param>
    public void SetTimeScale(float scale) => timeScale = scale;

    /// <summary>
    /// Updates the timer, should be called every frame in the Update method.
    /// </summary>
    /// <returns>True if the timer has elapsed; false otherwise.</returns>
    public bool UpdateTimer()
    {
        if (!isRunning) return false;

        elapsedTime += Time.deltaTime * timeScale;

        if (elapsedTime >= duration)
        {
            if (autoReset)
            {
                elapsedTime = 0f; // Reset for next cycle
            }
            else
            {
                isRunning = false;
            }

            OnComplete?.Invoke(); // Trigger the callback when the timer completes
            return true; // Timer has elapsed
        }

        return false; // Timer is still running
    }

    /// <summary>
    /// Checks if the timer has elapsed its duration without stopping it.
    /// </summary>
    /// <returns>True if the duration has elapsed; otherwise, false.</returns>
    public bool HasElapsed() => elapsedTime >= duration;

    /// <summary>
    /// Returns the remaining time in seconds before the timer elapses.
    /// </summary>
    /// <returns>Time left in seconds.</returns>
    public float GetRemainingTime() => Mathf.Max(0f, duration - elapsedTime);

    /// <summary>
    /// Returns the percentage of the timer's duration that has elapsed.
    /// </summary>
    /// <returns>A value between 0 and 1 representing the elapsed percentage.</returns>
    public float GetElapsedPercentage() => Mathf.Clamp01(elapsedTime / duration);

    /// <summary>
    /// Adjusts the timer's duration while it is running or paused.
    /// </summary>
    /// <param name="newDuration">New duration for the timer in seconds.</param>
    public void AdjustDuration(float newDuration) => duration = newDuration;

    /// <summary>
    /// Starts the timer at a specific elapsed time. Useful for resuming from a saved state.
    /// </summary>
    /// <param name="initialElapsedTime">The elapsed time to start from.</param>
    public void StartAt(float initialElapsedTime)
    {
        elapsedTime = Mathf.Clamp(initialElapsedTime, 0f, duration);
        isRunning = true;
    }

    #endregion
}
