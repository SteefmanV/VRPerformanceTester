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

    public List<TestData> _testQueue = new List<TestData>();
    private List<TestData> _testResults = new List<TestData>();

    private int _totalTests = 0;


    void Start()
    {
        _totalTests = _testQueue.Count;
        RunNextTest();
    }


    private void RunNextTest()
    {
        log("Test Started running!");

        if (_testQueue.Count > 0)
        {
            TestData nextTest = _testQueue[0];
            StartCoroutine(runTest(nextTest));
            _testQueue.Remove(nextTest);

            _testRunningText.text = "<color=orange>Test Running - " + (_totalTests - _testQueue.Count) + " / " + _totalTests + "</color>";
            log(_testQueue.Count + " more tests in queue", "yellow");
        }
        else
        {
            LogExporter.ExportTestDataToExcel(_testResults);
            _testRunningText.text = "<color=green> Done testing!</color>";
            log("All tests are done!");
        }
    }


    private IEnumerator runTest(TestData pData)
    {
        log("Preparing new text...", "yellow");

        TestData testData = new TestData(pData);
        _objectSpawner.CleanObjects();

        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => _objectSpawner.GenerateObjectGrid(testData.gridSize, testData.type)); // Generate object grid

        testData.trisCount = _objectSpawner.GetTrisCount();

        yield return new WaitForSeconds(5); // Give it some extra time to load 
        log("Next test: " + testData.testDescription, "yellow");

        // Recording test
        _performanceLogger.StartRecording();
        yield return new WaitForSeconds(testData.testDurationSeconds);
        _performanceLogger.StopRecording();

        // Save test results
        testData.frameData = _performanceLogger.getFpsLog();
        _testResults.Add(testData);

        RunNextTest();
    }


    private void log(string pMessage, string pColor = "green")
    {
        Debug.Log("<color=" + pColor + ">" + pMessage + "</color>");
    }
}
