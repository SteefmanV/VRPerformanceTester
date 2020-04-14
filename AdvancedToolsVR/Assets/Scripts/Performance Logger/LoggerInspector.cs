using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PerformanceLogger))]
public class LoggerInspector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsCounter = null;

    [Header("Stats:")]
    [SerializeField] private bool _isRecording;
    [SerializeField] private float _currentFps = 0;
    [SerializeField] private float _minFps = 0;
    [SerializeField] private float _maxFps = 0;
    [SerializeField] private float _averageFps = 0;
    [SerializeField] private float _frameCount = 0;

    private PerformanceLogger _logger;


    private void Start()
    {
        _logger = GetComponent<PerformanceLogger>();
        _logger.NewFrame += onNewFrame;
    }


    void Update()
    {
        _isRecording = _logger.isRecording;

        if(!_isRecording && _fpsCounter.gameObject.activeSelf)
            _fpsCounter.gameObject.SetActive(false);
        else if (_isRecording && !_fpsCounter.gameObject.activeSelf)
            _fpsCounter.gameObject.SetActive(true);
    }


    private void resetData()
    {
        _minFps = 0;
        _maxFps = 0;
        _averageFps = 0;
        _frameCount = 0;
        _currentFps = 0;
    }


    private void onNewFrame(object pSender, EventArgs pE)
    {
        _minFps = _logger.minFps;
        _maxFps = _logger.maxFps;
        _averageFps = _logger.averageFps();
        _frameCount = _logger.totalFramesRecorded;
        _currentFps = _logger.currentFPS;
        updateFPSCounter(_currentFps);
    }


    private void updateFPSCounter(float pCurrentFps)
    {
        if (_fpsCounter == null) return;

        string fpsString;
        if (pCurrentFps > 85) fpsString = "<color=green>" + pCurrentFps.ToString("0");
        else if (pCurrentFps > 60) fpsString = "<color=orange>" + pCurrentFps.ToString("0");
        else fpsString = "<color=red>" + pCurrentFps.ToString("0");

        _fpsCounter.text = fpsString;
    }
}
