using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTest", menuName = "PerformanceTest", order = 1)]
public class TestData : ScriptableObject
{
    // Fill in test data
    public string testName;
    public string testDescription;
    public int testDurationSeconds;
    public ObjectSpawner.ObjectType type;
    public Vector3Int gridSize;

    // This will be automatically generates 
    public List<FrameInformation> frameData;
    public long trisCount { get; set; } = 0;
    public int objectCount { get; set; } = 0;

    public TestData(TestData pCopy)
    {
        testName = pCopy.testName;
        testDescription = pCopy.testDescription;
        testDurationSeconds = pCopy.testDurationSeconds;
        type = pCopy.type;
        gridSize = pCopy.gridSize;
        frameData = pCopy.frameData;
        trisCount = pCopy.trisCount;
        objectCount = pCopy.objectCount;
    }

    public float GetAverageFPS()
    {
        float totalFPS = 0;
        foreach(FrameInformation frameInfo in frameData)
        {
            totalFPS += frameInfo.frameFPS;
        }

        return totalFPS / frameData.Count;
    }
}
