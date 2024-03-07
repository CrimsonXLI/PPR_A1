using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatBarIncrement : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private StatBar _statBar;
    
    public Image Image => _image;

    public void OnPointerEnter(PointerEventData eventData)
    {
#if UNITY_EDITOR
        _statBar.Hover(this);
#endif
    }

    public void OnPointerClick(PointerEventData eventData)
    {
#if UNITY_EDITOR
        _statBar.SetValue(this);
#endif
    }

    public void OnPointerExit(PointerEventData eventData)
    {
#if UNITY_EDITOR
        _statBar.Unhover();
#endif
    }
}
