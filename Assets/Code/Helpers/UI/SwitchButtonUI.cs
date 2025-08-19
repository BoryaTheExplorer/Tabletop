using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchButtonImageUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Graphic _target;

    [SerializeField] private Color _onHoverColor;
    [SerializeField] private Color _onClickColor;
    private Color _baseColor;

    public bool Clicked = false;

    private void Start()
    {
        _baseColor = Color.white;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        _target.color = _onClickColor;

        Clicked = !Clicked;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        _target.color = _onHoverColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!Clicked)
        {
            _target.color = _baseColor;
        }
        else
        {
            _target.color = _onClickColor;
        }
    }
    public void ResetColor()
    {
        Clicked = false;
        _target.color = _baseColor;
    }
}
