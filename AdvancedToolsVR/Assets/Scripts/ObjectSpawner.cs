﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { cube12, s100, s1k, s10k, s100k }

    [Header("Object Prefabs")]
    [SerializeField] private GameObject _cube12 = null;
    [SerializeField] private GameObject _sphere100 = null;
    [SerializeField] private GameObject _sphere1k = null;
    [SerializeField] private GameObject _sphere10k = null;
    [SerializeField] private GameObject _sphere100k = null;

    [Header("Spawner Settings:")]
    [SerializeField] private float _objectOffset = 10;


    public bool GenerateObjectGrid(Vector3Int pGridSize, ObjectType pType)
    {
        Vector3 centerOffset = new Vector3(pGridSize.x, pGridSize.y, pGridSize.z) * _objectOffset  * 0.5f;

        for (int x = 0; x < pGridSize.x; ++x)
        {
            for (int y = 0; y < pGridSize.y; ++y)
            {
                for (int z = 0; z < pGridSize.z; ++z)
                {
                    spawnObject(pType, (new Vector3(x, y, z) * _objectOffset) - centerOffset);
                }
            }
        }

        return true;
    }


    private void spawnObject(ObjectType pType, Vector3 pPositoin)
    {
        GameObject objectType = null;
        switch (pType)
        {
            case ObjectType.cube12:
                objectType = _cube12;
                break;
            case ObjectType.s100:
                objectType = _sphere100;
                break;
            case ObjectType.s1k:
                objectType = _sphere1k;
                break;
            case ObjectType.s10k:
                objectType = _sphere10k;
                break;
            case ObjectType.s100k:
                objectType = _sphere100k;
                break;
        }

        Instantiate(objectType, pPositoin, Quaternion.identity, transform);
    }


    public void CleanObjects()
    {
        foreach (Transform trans in transform)
        {
            Destroy(trans.gameObject);
        }
    }


    public int GetTrisCount()
    {
        int trisCount = 0;

        MeshFilter[] meshFilters = FindObjectsOfType<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            trisCount += meshFilter.mesh.triangles.Length / 3; // Devide by 3 because 1 triangle is made up of 3 ints
        }

        return trisCount;
    }
}
