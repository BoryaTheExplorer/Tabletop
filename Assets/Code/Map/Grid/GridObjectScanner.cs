using UnityEngine;
using UnityEngine.Splines;

public static class GridObjectScanner
{
    private static Camera _camera;
    
    static GridObjectScanner()
    {
        _camera = Camera.main;
    }

    public static bool Scan(out GridObject gridObject, out Vector3Int position)
    {
        Ray ray = _camera.ScreenPointToRay(GameInput.Instance.GetPointerScreenPosition());
        gridObject = null;
        position = new Vector3Int(-1, -1, -1);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return false;
        }

        int x = Mathf.FloorToInt(hit.point.x + .5f + (hit.normal * .5f).x);
        int y = Mathf.FloorToInt(hit.point.y + .5f + (hit.normal * .5f).y);
        int z = Mathf.FloorToInt(hit.point.z + .5f + (hit.normal * .5f).z);

        position = new Vector3Int(x, y, z);

        if (!hit.transform.TryGetComponent<GridObject>(out gridObject))
        {
            return false;
        }

        return true;

    }
}
