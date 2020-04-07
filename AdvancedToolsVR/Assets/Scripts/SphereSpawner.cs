using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public enum Sphere { s100, s1k, s10k, s100k }

    [Header("Sphere Prefabs")]
    [SerializeField] private GameObject _sphere100 = null;
    [SerializeField] private GameObject _sphere1k = null;
    [SerializeField] private GameObject _sphere10k = null;
    [SerializeField] private GameObject _sphere100k = null;

    [Header("Spawner Settings:")]
    [SerializeField] private float _objectOffset = 10;


    public bool GenerateSphereGrid(Vector3Int pGridSize, Sphere pType)
    {
        Vector3 centerOffset = new Vector3(pGridSize.x, pGridSize.y, pGridSize.z) * _objectOffset  * 0.5f;
        for (int x = 0; x < pGridSize.x; ++x)
        {
            for (int y = 0; y < pGridSize.y; ++y)
            {
                for (int z = 0; z < pGridSize.z; ++z)
                {
                    spawnSphere(pType, (new Vector3(x, y, z) * _objectOffset) - centerOffset);
                }
            }
        }

        return true;
    }


    private void spawnSphere(Sphere sphere, Vector3 position)
    {
        GameObject sphereObject = null;
        switch (sphere)
        {
            case Sphere.s100:
                sphereObject = _sphere100;
                break;
            case Sphere.s1k:
                sphereObject = _sphere1k;
                break;
            case Sphere.s10k:
                sphereObject = _sphere10k;
                break;
            case Sphere.s100k:
                sphereObject = _sphere100k;
                break;
        }

        Instantiate(sphereObject, position, Quaternion.identity, transform);
    }

    public void Clean()
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
