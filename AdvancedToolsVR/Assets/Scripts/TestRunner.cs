using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRunner : MonoBehaviour
{
    [SerializeField] private SphereSpawner _sphereSpawner = null;
    [SerializeField] private PerformanceLogger _performanceLogger = null;

    [Header("Test information")]
    [SerializeField] private string _currentRunningTest = "";
    [SerializeField] private float secondsUntilTestComplete = 0;
    private Queue<TestData> testQueue = new Queue<TestData>();

    private List<TestData> testResults = new List<TestData>();
    [SerializeField] private bool _testRunning;

    void Start()
    {
        TestData lowTest = new TestData()
        {
            testName = "SimpleTest",
            testDescription = "Simple test with a few small objects",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(10, 10, 10),
            type = SphereSpawner.Sphere.s100
        };

        testQueue.Enqueue(lowTest);

        TestData lowWithALotOfObject = new TestData()
        {
            testName = "LotOfObjects",
            testDescription = "A LOT OF small spheres",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(20, 20, 20),
            type = SphereSpawner.Sphere.s100
        };

        testQueue.Enqueue(lowWithALotOfObject);

        TestData mediumTest = new TestData()
        {
            testName = "MediumTest",
            testDescription = "Medium test: 10,10,10 with 10k Spheres",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(10, 10, 10),
            type = SphereSpawner.Sphere.s100k
        };
        testQueue.Enqueue(mediumTest);

        TestData hardcoreTest = new TestData()
        {
            testName = "HardcoreTest",
            testDescription = "Hardcore test: 20,10,10 with 100k Spheres",
            testDurationSeconds = 15,
            gridSize = new Vector3Int(20, 10, 10),
            type = SphereSpawner.Sphere.s100k
        };

        testQueue.Enqueue(hardcoreTest);

        StartCoroutine(StartRunningTests());
    }

    private void Update()
    {
        if (_testRunning) secondsUntilTestComplete -= Time.deltaTime;
    }


    private IEnumerator StartRunningTests()
    {
        Debug.Log("<color=green> Test Started running!</color>");


        secondsUntilTestComplete = 0;
        foreach (TestData test in testQueue)
        {
            secondsUntilTestComplete += test.testDurationSeconds;
        }

        while (testQueue.Count > 0)
        {
            _testRunning = true;
            StartCoroutine(runTest(testQueue.Dequeue()));
            Debug.Log("<color=yellow>" + testQueue.Count + " more testsin queue </color>");
            yield return new WaitWhile(() => _testRunning);
        }

        LogExporter.ExportLogToExcel(testResults);

        Debug.Log("<color=green> All tests are done! </color>");
    }


    private IEnumerator runTest(TestData pTestData)
    {
        _sphereSpawner.Clean();
        yield return null;
        // Generate grid of spheres and save the triangle & object count
        yield return new WaitUntil(() => _sphereSpawner.GenerateSphereGrid(pTestData.gridSize, pTestData.type));
        yield return new WaitForSeconds(5); // Give it some extra time to load
        pTestData.trisCount = _sphereSpawner.GetTrisCount();
        pTestData.objectCount = pTestData.gridSize.x * pTestData.gridSize.y * pTestData.gridSize.z;
        _currentRunningTest = pTestData.testDescription;
        Debug.Log("<color=yellow> Next test: " + pTestData.testDescription +"</color>");
        yield return null;


        _performanceLogger.StartRecording(); // Start recording the fps
        yield return new WaitForSeconds(pTestData.testDurationSeconds);
        _performanceLogger.StopRecording();// Stop recording when test done

        // Save test results
        pTestData.frameData = _performanceLogger.getFpsLog();
        testResults.Add(pTestData);
        _testRunning = false;
    }
}
