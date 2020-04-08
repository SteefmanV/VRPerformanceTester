using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TestData
{
    // Fill in test data
    public string testName;
    public string testDescription;
    public int testDurationSeconds;
    public ObjectSpawner.ObjectType type;
    public Vector3Int gridSize;

    // This will be automatically generates 
    public List<FrameInformation> frameData;
    public int trisCount;   
    public int objectCount;
}
