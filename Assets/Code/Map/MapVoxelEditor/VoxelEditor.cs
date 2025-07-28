using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class VoxelEditor : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private NetworkMap _networkMap;
    [SerializeField] private GameObject _debugObject;
    [SerializeField] private VoxelType _voxelToPlace;
    [SerializeField] private VoxelEditorBrushType _brushType;
    [SerializeField][Range(1, 5)] private int _brushSize;
    private Dictionary<Vector3Int, VoxelType> _history = new Dictionary<Vector3Int, VoxelType>();
    
    private void OnEnable()
    {
        _history.Clear();

        GameInput.Instance.OnClickPerformed += GameInput_OnClickPerformed;
        GameInput.Instance.OnRightClickPerformed += GameInput_OnRightClickPerformed;
    }
    private void GameInput_OnRightClickPerformed()
    {
        _ = Paint(VoxelEditorAction.Remove, VoxelType.Air);
    }

    private void GameInput_OnClickPerformed()
    {
        _ = Paint(VoxelEditorAction.Place, _voxelToPlace);
    }

    private async Task Paint(VoxelEditorAction action, VoxelType voxel)
    {
        await Awaitable.NextFrameAsync();

        Vector3 pos;
        Vector3 normal;
        VoxelMapScanner.Scan(out pos, out normal);

        switch (_brushType)
        {
            case VoxelEditorBrushType.Single:
                _ = EditVoxelAt(action, voxel, pos, normal);
                break;
            case VoxelEditorBrushType.Sphere:
                for (int x = -_brushSize + 1; x < _brushSize; x++)
                {
                    for (int y = -_brushSize + 1; y < _brushSize; y++)
                    {
                        for (int z = -_brushSize + 1; z < _brushSize; z++)
                        {

                            if (Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2) <= Mathf.Pow(_brushSize, 2))
                            _ = EditVoxelAt(action, voxel, pos + new Vector3(x, y ,z), normal);
                        }
                    }
                }
                break;
        }
    }
    private async Task EditVoxelAt(VoxelEditorAction action, VoxelType voxel, Vector3 hitPosition, Vector3 hitNormal)
    {
        await Awaitable.NextFrameAsync();

        Vector3Int voxelPosition;
        ChunkData chunkData;

        if (action == VoxelEditorAction.Place && _brushType == VoxelEditorBrushType.Single)
        {
            hitPosition += hitNormal;
        }
        else if (_brushType == VoxelEditorBrushType.Single)
        {
            hitPosition -= hitNormal;
        }

        hitPosition += new Vector3(.5f, .5f, .5f);

        if (hitPosition.x < 0)
            return;

        chunkData = _map.GetChunkDataFromWorldCoordinates(hitPosition);

        if (chunkData == null)
            return;

        int x = Mathf.FloorToInt(hitPosition.x);
        int y = Mathf.FloorToInt(hitPosition.y);
        int z = Mathf.FloorToInt(hitPosition.z);

        voxelPosition = new Vector3Int(x, y, z);
        if (!_history.ContainsKey(voxelPosition))
        {
            _history.Add(voxelPosition, _map.GetVoxelFromChunkCoordinates(chunkData, voxelPosition.x, voxelPosition.y, voxelPosition.z));
        }

        voxelPosition = voxelPosition - chunkData.WorldPosition;

        Chunk.SetVoxel(chunkData, voxelPosition, voxel);
        chunkData.Modified = true;

        _map.ReconstructModifiedChunk(chunkData, voxelPosition);
        _networkMap.SendUpdateMapRPC(new NetworkVoxelData
        {
            Voxel = voxel,

            WorldX = chunkData.WorldPosition.x,
            WorldY = chunkData.WorldPosition.y,
            WorldZ = chunkData.WorldPosition.z,

            VoxelX = voxelPosition.x,
            VoxelY = voxelPosition.y,
            VoxelZ = voxelPosition.z
        });
    }
    //Used by a Button
    public void RestoreMapButton()
    {
        _ = RestoreMap();
    }
    public async Task RestoreMap()
    {
        await Awaitable.NextFrameAsync();

        Vector3Int voxelPos;
        ChunkData data;
        
        foreach (var kvp in _history)
        {
            data = _map.GetChunkDataFromWorldCoordinates(kvp.Key);

            if (data == null)
                return;

            voxelPos = kvp.Key - data.WorldPosition;

            Chunk.SetVoxel(data, voxelPos, kvp.Value);
            data.Modified = true;

            _map.ReconstructModifiedChunk(data, voxelPos);
            _networkMap.SendUpdateMapRPC(new NetworkVoxelData
            {
                Voxel = kvp.Value,

                WorldX = data.WorldPosition.x,
                WorldY = data.WorldPosition.y,
                WorldZ = data.WorldPosition.z,

                VoxelX = voxelPos.x,
                VoxelY = voxelPos.y,
                VoxelZ = voxelPos.z
            });
        }
    }

    private void OnDisable()
    {
        GameInput.Instance.OnClickPerformed -= GameInput_OnClickPerformed;
        GameInput.Instance.OnRightClickPerformed -= GameInput_OnRightClickPerformed;
    }
}
