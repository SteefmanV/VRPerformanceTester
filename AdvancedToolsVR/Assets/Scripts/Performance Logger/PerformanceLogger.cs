using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceLogger : MonoBehaviour
{
    public float currentFPS { get; private set; } = 0;

    public float minFps { get; private set; } = 0;
    public float maxFps { get; private set; } = 0;
    public int totalFramesRecorded { get; private set; } = 0;


    [SerializeField] private int fpsUpdateRate = 4;
    private List<FrameInformation> _frameLog = new List<FrameInformation>();
    public bool isRecording { get; private set; } = false;
    private float _timeRecording = 0;

    private float _timeBetweenUpdates = 0;
    private int _frameCount = 0;


    private void Update()
    {
        if (isRecording)
        {
            updateFPS();
        }
    }


    /// <summary>
    /// Start recording FPS
    /// </summary>
    public void StartRecording()
    {
        // Reset all data
        _frameLog.Clear();
        minFps = float.MaxValue;
        maxFps = float.MinValue;
        totalFramesRecorded = 0;
        _timeRecording = 0;
        _timeBetweenUpdates = 0;
        _frameCount = 0;

        isRecording = true;
    }


    /// <summary>
    /// Stop recording FPS
    /// </summary>
    public void StopRecording()
    {
        isRecording = false;
    }


    /// <summary>
    /// Get average FPS of recording
    /// </summary>
    public float averageFps()
    {
        return totalFramesRecorded / _timeRecording;
    }


    /// <summary>
    /// Get current FPS
    /// </summary>
    public float GetCurrentFps()
    {
        return currentFPS;
    }


    /// <summary>
    /// Returns a copy of the fps log
    /// </summary>
    public List<FrameInformation> getFpsLog()
    {
        return new List<FrameInformation>(_frameLog);
    }


    private void updateFPS()
    {
        _timeRecording += Time.unscaledDeltaTime;
        _timeBetweenUpdates += Time.unscaledDeltaTime;

        ++_frameCount;
        ++totalFramesRecorded;

        if (_timeBetweenUpdates > 1.0 / fpsUpdateRate)
        {
            currentFPS = _frameCount / _timeBetweenUpdates;
            _frameCount = 0;
            _timeBetweenUpdates -= 1.0f / fpsUpdateRate; // avoids skipping a frame

            logFps();
        }
    }


    private void logFps()
    {
        if(currentFPS == 0) return; 
        if (currentFPS < minFps) minFps = currentFPS;
        if (currentFPS > maxFps) maxFps = currentFPS;
        _frameLog.Add(new FrameInformation(totalFramesRecorded, currentFPS, Time.deltaTime));
    }
}
