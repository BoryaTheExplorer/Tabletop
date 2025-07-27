using UnityEngine;

public class VoxelEditor : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private NetworkMap _networkMap;
    [SerializeField] private GameObject _debugObject;
    [SerializeField] private VoxelType _voxelToPlace;
    private void Start()
    {
        //GameInput.Instance.OnClickPerformed += GameInput_OnClickPerformed;
        //GameInput.Instance.OnRightClickPerformed += GameInput_OnRightClickPerformed;
    }
    private void OnEnable()
    {
        GameInput.Instance.OnClickPerformed += GameInput_OnClickPerformed;
        GameInput.Instance.OnRightClickPerformed += GameInput_OnRightClickPerformed;
    }
    private void GameInput_OnRightClickPerformed()
    {
        EditVoxelPointedAt(VoxelEditorAction.Remove, VoxelType.Air);
    }

    private void GameInput_OnClickPerformed()
    {
        EditVoxelPointedAt(VoxelEditorAction.Place, _voxelToPlace);
    }

    private void EditVoxelPointedAt(VoxelEditorAction action, VoxelType voxel)
    {
        Vector3 pos;
        Vector3 normal;

        VoxelMapScanner.Scan(out pos, out normal);
        
        if (action == VoxelEditorAction.Place)
        {
            pos += normal;
        }
        else
        {
            pos -= normal;
        }

        pos += new Vector3(.5f, .5f, .5f);

        if (pos.x < 0)
            return;

        ChunkData data = _map.GetChunkDataFromWorldCoordinates(pos);

        if (data == null)
            return;

        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        Vector3Int voxelPos = new Vector3Int(x, y, z);
        voxelPos = voxelPos - data.WorldPosition;

        Chunk.SetVoxel(data, voxelPos, voxel);
        data.Modified = true;

        _map.ReconstructModifiedChunk(data, voxelPos);
        _networkMap.SendUpdateMapRPC(new NetworkVoxelData
        {
            Voxel = voxel,

            WorldX = data.WorldPosition.x,
            WorldY = data.WorldPosition.y,
            WorldZ = data.WorldPosition.z,

            VoxelX = voxelPos.x,
            VoxelY = voxelPos.y,
            VoxelZ = voxelPos.z
        });
    }

    private void OnDisable()
    {
        GameInput.Instance.OnClickPerformed -= GameInput_OnClickPerformed;
        GameInput.Instance.OnRightClickPerformed -= GameInput_OnRightClickPerformed;
    }
}
