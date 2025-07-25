using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Mesh _mesh;

    private bool _showGizmos = false;

    public ChunkData ChunkData { get; private set; }

    public bool Modified
    {
        get
        {
            return ChunkData.Modified;
        }
        set
        {
            ChunkData.Modified = value;
        }
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        _mesh = _meshFilter.mesh;
    }

    public void InitChunk(ChunkData chunkData)
    {
        ChunkData = chunkData;
    }

    private void RenderMesh(MeshData meshData)
    {
        _mesh.Clear();

        _mesh.subMeshCount = 2;
        _mesh.vertices = meshData.Vertices.Concat(meshData.SubMesh.Vertices).ToArray();

        _mesh.SetTriangles(meshData.Triangles.ToArray(), 0);
        _mesh.SetTriangles(meshData.SubMesh.Triangles.Select(val => val + meshData.Vertices.Count).ToArray(), 1);

        _mesh.uv = meshData.UVs.Concat(meshData.SubMesh.UVs).ToArray();
        _mesh.RecalculateNormals();

        _meshCollider.sharedMesh = null;
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = meshData.ColliderVertices.ToArray();
        collisionMesh.triangles = meshData.ColliderTriangles.ToArray();
        collisionMesh.RecalculateNormals();

        _meshCollider.sharedMesh = collisionMesh;
    } 

    public void UpdateChunk()
    {
        RenderMesh(Chunk.GetChunkMeshData(ChunkData));
    }

    public void UpdateChunk(MeshData meshData)
    {
        RenderMesh(meshData);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_showGizmos) return;

        if (Application.isPlaying && ChunkData != null)
        {
            if (Selection.activeObject == gameObject)
            {
                Gizmos.color = new Color(0f, 1f, 0f, .2f);
            }
            else
            {
                Gizmos.color = new Color(1f, 0f, 0f, .2f);
            }

            Gizmos.DrawCube(transform.position + new Vector3(ChunkData.ChunkSize / 2f, ChunkData.ChunkHeight / 2f, ChunkData.ChunkSize / 2f),
                                                 new Vector3(ChunkData.ChunkSize, ChunkData.ChunkHeight, ChunkData.ChunkSize));
        }
    }
#endif
}
