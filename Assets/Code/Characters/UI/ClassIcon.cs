using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private ClassSpriteDictionarySO _spriteDictionarySO;

    public void Init(CharacterClassKey classKey, int level)
    {
        if (_spriteDictionarySO != null)
        {
            if (_spriteDictionarySO.ClassSprites.ContainsKey(classKey))
                _image.sprite = _spriteDictionarySO.ClassSprites[classKey];
        }

        _level.text = level.ToString();
    }
}
