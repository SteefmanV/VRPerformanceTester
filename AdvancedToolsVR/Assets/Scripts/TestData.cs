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
    public int trisCount { get; set; }
    public int objectCount { get; set; }
}
