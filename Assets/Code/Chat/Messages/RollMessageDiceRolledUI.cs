using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollMessageDiceRolledUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _count;

    public void Setup(Sprite sprite, int count)
    {
        _image.sprite = sprite;
        _count.text = count.ToString();
    }
}
