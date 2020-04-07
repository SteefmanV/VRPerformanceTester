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

    [SerializeField] private Vector3Int _gridSize = new Vector3Int();
    [SerializeField] private Sphere _sphereType = Sphere.s100;
    [SerializeField] private float _objectOffset = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) generateSphereGrid();
    }
    private void generateSphereGrid()
    {
        clean();

        for (int x = 0; x < _gridSize.x; ++x)
        {
            for (int y = 0; y < _gridSize.x; ++y)
            {
                for (int z = 0; z < _gridSize.x; ++z)
                {
                    spawnSphere(_sphereType, new Vector3(x , y, z) * _objectOffset);
                }
            }
        }
    }
    

    private void spawnSphere(Sphere sphere, Vector3 position)
    {
        GameObject sphereObject = null;
        switch(sphere)
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

    private void clean()
    {
        foreach(Transform trans in transform)
        {
            Destroy(trans.gameObject);
        }
    }
}
