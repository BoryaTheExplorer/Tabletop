using UnityEngine;

public static class VoxelMapScanner
{
    private static Camera _camera;
    static VoxelMapScanner()
    {
        _camera = Camera.main;
    }
    public static void Scan(out Vector3 point, out Vector3 normal)
    {
        Ray ray = _camera.ScreenPointToRay(GameInput.Instance.GetPointerScreenPosition());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            point = hit.point;
            normal = hit.normal * .5f;
            return;
        }

        point = new Vector3(-1f, -1f, -1f);
        normal = Vector3.zero;
    }
}
