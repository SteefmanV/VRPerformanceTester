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

    private void Update()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
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


    private IEnumerator runTest(TestData pData)
    {
        TestData testData = new TestData(pData);
        _objectSpawner.CleanObjects();
        Debug.Log("<color=yellow> Preparing new text... </color>");

        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => _objectSpawner.GenerateObjectGrid(testData.gridSize, testData.type)); // Generate object grid

        testData.objectCount = testData.gridSize.x * testData.gridSize.y * testData.gridSize.z;
        testData.trisCount = _objectSpawner.GetTrisCount();

        yield return new WaitForSeconds(5); // Give it some extra time to load
        Debug.Log("<color=yellow> Next test: " + testData.testDescription +"</color>");

        // Recording test
        _performanceLogger.StartRecording();
        yield return new WaitForSeconds(testData.testDurationSeconds);
        _performanceLogger.StopRecording();

        // Save test results
        testData.frameData = _performanceLogger.getFpsLog();
        _testResults.Add(testData);

        RunNextTest();
    }
}
