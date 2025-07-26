using UnityEngine;

public static class VoxelMapScanner
{
    public static void Scan(out Vector3 point, out Vector3 normal)
    {
        Ray ray = Camera.main.ScreenPointToRay(GameInput.Instance.GetPointerScreenPosition());

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
