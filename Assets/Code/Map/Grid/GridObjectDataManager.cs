using UnityEngine;

public class GridObjectDataManager : MonoBehaviour
{
    [SerializeField] private MiniaturesSO _miniaturesSO;
    public static MiniaturesSO MiniaturesSO;

    private void Awake()
    {
        if (_miniaturesSO != null)
            MiniaturesSO = _miniaturesSO;
    }
}
