using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _popup;
    [SerializeField] private TMP_Text _popupText;
    [SerializeField] private Button _popupButton;
    
    public void Display(string text, Action onAcceptedCallback = null)
    {
        _popupText.text = text;
        
        _background.SetActive(true);
        _popup.SetActive(true);
        
        _popupButton.onClick.RemoveAllListeners();
        _popupButton.onClick.AddListener(() => OnAcceptButtonClicked(onAcceptedCallback));
    }
    
    private void OnAcceptButtonClicked(Action callback = null)
    {
        _popupButton.onClick.RemoveAllListeners();

        Hide();
        
        callback?.Invoke();
    }

    private void Hide()
    {
        _popupText.text = "";
        
        _background.SetActive(false);
        _popup.SetActive(false);
    }
}
