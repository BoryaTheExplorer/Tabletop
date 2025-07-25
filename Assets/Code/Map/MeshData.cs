using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
    public List<int> Triangles { get; private set; } = new List<int>();
    public List<Vector2> UVs { get; private set; } = new List<Vector2>();

    public List<Vector3> ColliderVertices { get; private set; } = new List<Vector3>();
    public List<int> ColliderTriangles { get; private set; } = new List<int>();

    public MeshData SubMesh;
    private bool _isMainMesh = true;

    public MeshData(bool isMainMesh)
    {
        if (isMainMesh)
        {
            SubMesh = new MeshData(false);
        }

        _isMainMesh = isMainMesh;
    }

    public void AddVertex(Vector3 vertex, bool generatesCollider)
    {
        Vertices.Add(vertex);
        
        if (generatesCollider)
        {
            ColliderVertices.Add(vertex);
        }
    }

    public void AddQuadTriangles(bool generatesCollider)
    {
        Triangles.Add(Vertices.Count - 4);
        Triangles.Add(Vertices.Count - 3);
        Triangles.Add(Vertices.Count - 2);

        Triangles.Add(Vertices.Count - 4);
        Triangles.Add(Vertices.Count - 2);
        Triangles.Add(Vertices.Count - 1);

        if (generatesCollider)
        {
            ColliderTriangles.Add(ColliderVertices.Count - 4);
            ColliderTriangles.Add(ColliderVertices.Count - 3);
            ColliderTriangles.Add(ColliderVertices.Count - 2);

            ColliderTriangles.Add(ColliderVertices.Count - 4);
            ColliderTriangles.Add(ColliderVertices.Count - 2);
            ColliderTriangles.Add(ColliderVertices.Count - 1);
        }
    }
}
