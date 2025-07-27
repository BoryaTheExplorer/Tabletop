using UnityEngine;

public class EditorToggle : MonoBehaviour
{
    [SerializeField] private VoxelEditor _editor;

    public void ToggleEditor()
    {
        if (_editor.gameObject)
        {
            _editor.gameObject.SetActive(!_editor.gameObject.activeSelf);
            return;
        }

        Debug.Log("???");
    }
}
