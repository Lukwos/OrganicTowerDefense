using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class FloorGenerator : MonoBehaviour
{
    public Vector2Int _size;
    public float _height;

    [Header("Perlin")]
    public float _scale;
    public Vector2 _offset;

    void OnValidate()
    {
        Mesh mesh = GenerateMesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    Mesh GenerateMesh()
    {

        var vertices = new Vector3[4 * _size.x * _size.y];
        var uv = new Vector2[4 * _size.x * _size.y];
        var triangles = new int[6 * _size.x * _size.y];

        int vi = 0, ti = 0;

        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                vertices[vi] = GetPoint(x, y);
                vertices[vi + 1] = GetPoint(x, y + 1);
                vertices[vi + 2] = GetPoint(x + 1, y);
                vertices[vi + 3] = GetPoint(x + 1, y + 1);

                uv[vi] = new Vector2(x, y);
                uv[vi + 1] = new Vector2(x, y + 1);
                uv[vi + 2] = new Vector2(x + 1, y);
                uv[vi + 3] = new Vector2(x + 1, y + 1);

                triangles[ti] = vi;
                triangles[ti + 1] = vi + 1;
                triangles[ti + 2] = vi + 2;

                triangles[ti + 3] = vi + 2;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = vi + 3;

                vi += 4;
                ti += 6;
            }
        }

        var mesh = new Mesh();
        mesh.name = "Generated Floor Mesh";
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    Vector3 GetPoint(float x, float y)
    {
        float perlin = Mathf.PerlinNoise(_scale * (x + _offset.x), _scale * (y + _offset.y));
        //perlin = Mathf.Round(perlin * 3 * _height) / (3 * _height);
        return new Vector3(x, _height * perlin, y);
    }
}
