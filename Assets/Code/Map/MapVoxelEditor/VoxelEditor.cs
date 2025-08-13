using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class VoxelEditor : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private NetworkMap _networkMap;
    [SerializeField] private GameObject _debugObject;
    [SerializeField] private VoxelType _voxelToPaint;
    [SerializeField] private VoxelEditorBrushType _brushType;
    [SerializeField][Range(1, 7)] private int _brushSize;
    public int BrushSizeMin { get; private set; } = 1;
    public int BrushSizeMax { get; private set; } = 7;
    public int BrushSize { get { return _brushSize; } }
    public VoxelEditorBrushType BrushType { get { return _brushType; } }
    public VoxelType VoxelToPaint {  get { return _voxelToPaint; } }

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
        _ = Paint(VoxelEditorAction.Place, _voxelToPaint);
    }

    private async Task Paint(VoxelEditorAction action, VoxelType voxel)
    {
        if (GameInput.Instance.IsPointerOverUI())
            return;
        await Awaitable.NextFrameAsync();

        Vector3 pos;
        Vector3 normal;
        VoxelMapScanner.Scan(out pos, out normal);

        switch (_brushType)
        {
            case VoxelEditorBrushType.Single:
                SearchAndEditVoxelAt(action, voxel, pos, normal);
                break;
            case VoxelEditorBrushType.Sphere:

                Vector3 offset = Vector3.zero;
                for (int x = -_brushSize + 1; x < _brushSize; x++)
                {
                    for (int y = -_brushSize + 1; y < _brushSize; y++)
                    {
                        for (int z = -_brushSize + 1; z < _brushSize; z++)
                        {
                            if (x * x + y * y + z * z > _brushSize * _brushSize) continue;
                            offset.Set(x, y, z);
                            SearchAndEditVoxelAt(action, voxel, pos + offset, normal);
                        }
                    }
                }
                break;
        }

        UpdateMap();
    }
    private void SearchAndEditVoxelAt(VoxelEditorAction action, VoxelType voxel, Vector3 hitPosition, Vector3 hitNormal)
    {
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

        ChunkData chunkData = _map.GetChunkDataFromWorldCoordinates(hitPosition);
        
        if (chunkData == null)
            return;

        int x = Mathf.FloorToInt(hitPosition.x);
        int y = Mathf.FloorToInt(hitPosition.y);
        int z = Mathf.FloorToInt(hitPosition.z);


        Vector3Int voxelPosition = new Vector3Int(x, y, z) - chunkData.WorldPosition;
        EditVoxelAt(voxel, voxelPosition, chunkData.WorldPosition, true);
    }
        
    public void EditVoxelAt(VoxelType voxel, Vector3Int localPosition, Vector3Int chunkPosition, bool saveInHistory)
    {
        ChunkData chunkData = _map.GetChunkDataFromWorldCoordinates(chunkPosition);

        if (chunkData == null)
            return;
        Vector3Int voxelWorldPosition = localPosition + chunkData.WorldPosition;
        
        if (!_history.ContainsKey(voxelWorldPosition) && saveInHistory)
        {
            VoxelType type = _map.GetVoxelFromChunkCoordinates(chunkData, voxelWorldPosition.x, voxelWorldPosition.y, voxelWorldPosition.z);
            _history.Add(voxelWorldPosition, type);
        }

        Chunk.SetVoxel(chunkData, localPosition, voxel);
        chunkData.Modified = true;

        _networkMap.SendEditVoxelRPC(new NetworkVoxelData
        {
            Voxel = voxel,

            ChunkPos = new NetworkVector3IntWrapper()
            {
                X = chunkData.WorldPosition.x,
                Y = chunkData.WorldPosition.y,
                Z = chunkData.WorldPosition.z
            },
            VoxelPos = new NetworkVector3IntWrapper()
            {
                X = localPosition.x,
                Y = localPosition.y,
                Z = localPosition.z
            }
        }, NetworkManager.Singleton.LocalClientId);
    
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
                continue;
            voxelPos = kvp.Key - data.WorldPosition;

            EditVoxelAt(kvp.Value, voxelPos, data.WorldPosition, false);
        }

        UpdateMap();
    }
    public void UpdateMap()
    {
        _map.UpdateModifiedChunks();
        _networkMap.SendUpdateMapRpc(NetworkManager.Singleton.LocalClientId);
    }

    public void SetBrushType(VoxelEditorBrushType brushType)
    {
        _brushType = brushType;
    }
    public void SetVoxelToPaint(VoxelType voxel)
    {
        _voxelToPaint = voxel;
    }
    public void SetBrushSize(int size)
    {
        _brushSize = size;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnClickPerformed -= GameInput_OnClickPerformed;
        GameInput.Instance.OnRightClickPerformed -= GameInput_OnRightClickPerformed;
    }
}
