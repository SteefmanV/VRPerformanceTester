using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[ExecuteInEditMode]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _defaultObject = null;
    [SerializeField] private int _objects = 0;
    [SerializeField] private int _trianglesPerObject = 0;
    [SerializeField] private float _zStrength = 0.5f;
    [SerializeField] private float _vertexDensity = 0.1f;

    public int verticeCount = 0;
    public int trisCount = 0;

    public int xSize = 20;
    public int ySize = 20;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearObjects();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            generateObject();
        }
    }


    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += () => ClearObjects();
        UnityEditor.EditorApplication.delayCall += () => generateObject();
#endif
    }

    private void generateObject()
    {
        GameObject newObject = Instantiate(_defaultObject, transform);
        Mesh mesh = new Mesh();
        newObject.GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        int i = 0;
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x * _vertexDensity, y * _vertexDensity, Mathf.PerlinNoise(x * 0.1f, y * 0.1f) * _zStrength);
                i++;
            }
        }

        int[] triangles = new int[xSize * ySize * 6];

        int vert = 0;
        int tris = 0;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        verticeCount = vertices.Length;
        trisCount = triangles.Length / 3;
    }


    private void ClearObjects()
    {
        List<Transform> tempList = transform.Cast<Transform>().ToList();

        foreach (Transform trans in tempList)
        {
            DestroyImmediate(trans.gameObject);
        }
    }
}
