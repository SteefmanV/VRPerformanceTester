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

    private Queue<TestData> _testQueue = new Queue<TestData>();
    private List<TestData> _testResults = new List<TestData>();

    private int _totalTests = 0;

    void Start()
    {
        TestData lowTest = new TestData()
        {
            testName = "SimpleTest",
            testDescription = "Simple test with a few small objects",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(10, 10, 10),
            type = ObjectSpawner.ObjectType.s100
        };

        _testQueue.Enqueue(lowTest);

        TestData lowWithALotOfObject = new TestData()
        {
            testName = "LotOfObjects",
            testDescription = "A LOT OF small spheres",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(20, 20, 20),
            type = ObjectSpawner.ObjectType.s100
        };

        _testQueue.Enqueue(lowWithALotOfObject);

        TestData mediumTest = new TestData()
        {
            testName = "MediumTest",
            testDescription = "Medium test: 10,10,10 with 10k Spheres",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(10, 10, 10),
            type = ObjectSpawner.ObjectType.s10k
        };
        _testQueue.Enqueue(mediumTest);

        TestData hardcoreTest = new TestData()
        {
            testName = "HardcoreTest",
            testDescription = "Hardcore test: 20,10,10 with 100k Spheres",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(20, 10, 10),
            type = ObjectSpawner.ObjectType.s100k
        };

        _testQueue.Enqueue(hardcoreTest);


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
            StartCoroutine(runTest(_testQueue.Dequeue()));
            _testRunningText.text = "<color=orange>Test Running - " + (_totalTests - _testQueue.Count) + " / " + _totalTests + "</color>";
            Debug.Log("<color=yellow>" + _testQueue.Count + " more tests in queue </color>");
            return;
        }

        LogExporter.ExportLogToExcel(_testResults);

        _testRunningText.text = "<color=green> Done testing!</color>";
        Debug.Log("<color=green> All tests are done! </color>");
    }


    private IEnumerator runTest(TestData pTestData)
    {
        _objectSpawner.Clean();
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
