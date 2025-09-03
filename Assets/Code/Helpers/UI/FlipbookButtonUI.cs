using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlipbookButtonUI<T> : MonoBehaviour, IPointerClickHandler where T : Enum
{
    [SerializeField] protected Image _targetImage;

    [SerializeField] protected SerializableDictionary<T, Sprite> _enumToSpriteSerializableDictionary;
    private Dictionary<T, Sprite> _spriteDictionary = new Dictionary<T, Sprite>();
    public Dictionary<T, Sprite> SpriteDictionary 
    { 
        get
        {
            if (_spriteDictionary.Count == 0)
                _spriteDictionary = _enumToSpriteSerializableDictionary.ToDictionary();

            return _spriteDictionary;
        } 
    }
    protected List<T> _availableKeys = new List<T>();
    protected int _currentKey = 0;
    
    public event Action<T> OnButtonPressed;
    
    protected virtual void Awake()
    {
        _availableKeys = SpriteDictionary.Keys.ToList();
        _targetImage.sprite = SpriteDictionary.Values.FirstOrDefault();
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (_currentKey >= _availableKeys.Count - 1)
            _currentKey = 0;
        else
            _currentKey++;

        FlipTo(_availableKeys[_currentKey]);
        OnButtonPressed?.Invoke(_availableKeys[_currentKey]);
    }

    public virtual void FlipTo(T val)
    {
        if (SpriteDictionary.ContainsKey(val))
            _targetImage.sprite = SpriteDictionary[val];
    }
}
