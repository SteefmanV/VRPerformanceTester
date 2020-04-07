using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PerformanceLogger))]
public class LoggerInspector : MonoBehaviour
{
    [SerializeField] private bool _recording = false;

    [Header("Stats:")]
    [SerializeField] private float _currentFps = 0;
    [SerializeField] private float _minFps = 0;
    [SerializeField] private float _maxFps = 0;
    [SerializeField] private float _averageFps = 0;
    [SerializeField] private float _frameCount = 0;

    private PerformanceLogger _logger;

    private void Start()
    {
        _logger = GetComponent<PerformanceLogger>();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            _recording = !_recording;
            if (_recording) restartLogger();
            else _logger.StopRecording();
        }

        if (_recording)
        {
            _minFps = _logger.minFps;
            _maxFps = _logger.maxFps;
            _averageFps = _logger.averageFps();
            _frameCount = _logger.frameCount;
            _currentFps = _logger.GetCurrentFps();
        }
    }

    private void restartLogger()
    {
        _minFps = 0;
        _maxFps = 0;
        _averageFps = 0;
        _frameCount = 0;
        _currentFps = 0;
        _logger.StartRecording();
    }
}
