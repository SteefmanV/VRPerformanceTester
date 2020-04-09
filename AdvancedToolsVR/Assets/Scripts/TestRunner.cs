using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestRunner : MonoBehaviour
{
    [SerializeField] private ObjectSpawner _objectSpawner = null;
    [SerializeField] private PerformanceLogger _performanceLogger = null;
    [SerializeField] private TextMeshProUGUI _testRunningText = null;

    [Header("Test information")]
    [SerializeField] private string _currentRunningTest = "";
    [SerializeField] private float secondsUntilTestComplete = 0;
    [SerializeField] private bool _testRunning;

    public List<TestData> _testQueue = new List<TestData>();
    private List<TestData> _testResults = new List<TestData>();

    private int _totalTests = 0;

    void Start()
    {
        _totalTests = _testQueue.Count;
        RunNextTest();
    }

    private void Update()
    {
        if (_testRunning) secondsUntilTestComplete -= Time.deltaTime;
    }


    private void RunNextTest()
    {
        Debug.Log("<color=green> Test Started running!</color>");

        if (_testQueue.Count > 0)
        {
            TestData nextTest = _testQueue[0];
            StartCoroutine(runTest(nextTest));
            _testQueue.Remove(nextTest);
            _testRunningText.text = "<color=orange>Test Running - " + (_totalTests - _testQueue.Count) + " / " + _totalTests + "</color>";
            Debug.Log("<color=yellow>" + _testQueue.Count + " more tests in queue </color>");
            return;
        }

        LogExporter.ExportTestDataToExcel(_testResults);

        _testRunningText.text = "<color=green> Done testing!</color>";
        Debug.Log("<color=green> All tests are done! </color>");
    }


    private IEnumerator runTest(TestData pTestData)
    {
        _objectSpawner.CleanObjects();
        yield return null;
        Debug.Log("<color=yellow> Preparing new text... </color>");

        yield return new WaitUntil(() => _objectSpawner.GenerateObjectGrid(pTestData.gridSize, pTestData.type)); // Generate object grid

        pTestData.trisCount = _objectSpawner.GetTrisCount();
        pTestData.objectCount = pTestData.gridSize.x * pTestData.gridSize.y * pTestData.gridSize.z;
        secondsUntilTestComplete += pTestData.testDurationSeconds;
        _currentRunningTest = pTestData.testDescription;

        yield return new WaitForSeconds(10); // Give it some extra time to load
        Debug.Log("<color=yellow> Next test: " + pTestData.testDescription +"</color>");

        // Recording test
        _testRunning = true;
        _performanceLogger.StartRecording();
        yield return new WaitForSeconds(pTestData.testDurationSeconds);
        _performanceLogger.StopRecording();
        _testRunning = false;

        // Save test results
        pTestData.frameData = _performanceLogger.getFpsLog();
        _testResults.Add(pTestData);

        RunNextTest();
    }
}
