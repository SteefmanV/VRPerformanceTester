using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceLogger : MonoBehaviour
{
    public float minFps { get; private set; } = 0;
    public float maxFps { get; private set; } = 0;
    public float frameCount { get; private set; } = 0;

    private List<float> _fpsLog = new List<float>();
    private bool _isRecording = false;
    private float _timeRecording = 0;


    private void Update()
    {
        if (_isRecording)
        {
            logFps();
            _timeRecording += Time.deltaTime;
        }
    }


    /// <summary>
    /// Start recording FPS
    /// </summary>
    public void StartRecording()
    {
        _fpsLog.Clear();
        _isRecording = true;
        minFps = float.MaxValue;
        maxFps = float.MinValue;
        _timeRecording = 0;
    }

    /// <summary>
    /// Stop recording FPS
    /// </summary>
    public void StopRecording()
    {
        _isRecording = false;
    }


    /// <summary>
    /// Get average FPS of recording
    /// </summary>
    public float averageFps()
    {
        return frameCount / _timeRecording;
    }


    /// <summary>
    /// Get current FPS
    /// </summary>
    public float GetCurrentFps()
    {
        return 1 / Time.deltaTime;
    }


    /// <summary>
    /// Returns a copy of the fps log
    /// </summary>
    public List<float> getFpsLog()
    {
        return new List<float>(_fpsLog);
    }


    private void logFps()
    {
        float fps = GetCurrentFps();
        if (fps < minFps) minFps = fps;
        if (fps > maxFps) maxFps = fps;
        ++frameCount;
        _fpsLog.Add(fps);
    }
}
