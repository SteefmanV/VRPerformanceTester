using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores infomation about a frame
/// </summary>
public struct FrameInformation
{
    public int frameNumber;
    public float frameFPS;
    public float frameTime;

    public FrameInformation(int pFrameNumber, float pFrameFPS, float pFrameTime)
    {
        frameNumber = pFrameNumber;
        frameFPS = pFrameFPS;
        frameTime = pFrameTime;
    }
}
