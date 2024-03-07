using System;
using Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

public class StatBar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatBarIncrement[] _increments;

    [Header("Prefabs")] [SerializeField] private StatBarPopup _popup;
    
    [Header("Settings")]
    [SerializeField] private Color _hoveredColour = new Color(0, 1, 0, 1f);
    [SerializeField] private Color _inactiveColour = new Color(1, 1, 1, 0.25f);

    private int _currentValue;
    private const int _maximumValue = 5;

    public event Action Emptied;
    public event Action OK;
    public event Action Filled;

    private void Awake()
    {
        if (_increments.Length != 5)
        {
            throw new Exception("StatBar must have 5 StatBarIncrements.");
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    // Value.
    //------------------------------------------------------------------------------------------------------------------

    public void ChangeValueBy(int valueDelta,  bool displayPopup = true)
    {
        int oldValue = _currentValue;
        int newValue = Mathf.Clamp(_currentValue + valueDelta, 1, _maximumValue);

        CheckValueChange(oldValue, newValue);
        
        _currentValue = newValue;
        
        if (displayPopup) _popup.Display(newValue - oldValue);
        
        UpdateUI();
    }

    public void SetValue(StatBarIncrement statBarIncrement)
    {
        int incrementIndex = Array.IndexOf(_increments, statBarIncrement);
        
        if (incrementIndex == -1)
        {
            throw new Exception("StatBarIncrement not found in StatBar.");
        }
        
        SetValue(incrementIndex + 1);
    }
    
    public void SetValue(int value, bool displayPopup = true)
    {
        int oldValue = _currentValue;
        int newValue = Mathf.Clamp(value, 1, _maximumValue);
        
        CheckValueChange(oldValue, newValue);
        
        _currentValue = newValue;
        
        if (displayPopup) _popup.Display(newValue - oldValue);
        
        UpdateUI();
    }
    
    private void CheckValueChange(int oldValue, int newValue)
    {
        switch (oldValue, newValue)
        {
            case (> 1, 1): //NOW EMPTY.
                Emptied?.Invoke();
                break;
            
            case (< 5, 5): //NOW FILLED.
                Filled?.Invoke();
                break;
            
            case (1, 2 or 3 or 4): //NOW OK.
            case (5, 4 or 3 or 2): //NOW OK.
                OK?.Invoke();
                break;
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    // UI.
    //------------------------------------------------------------------------------------------------------------------

    [Button]
    public void UpdateUI()
    {
        for (int i = 0; i < _increments.Length; i++)
        {
            _increments[i].Image.color = i < _currentValue ? Color.white : _inactiveColour;
        }
    }

    public void Hover(StatBarIncrement increment)
    {
        for (int index = 0; index < _increments.Length; index++)
        {
            StatBarIncrement matchingIncrement = _increments[index];
            
            if (matchingIncrement == increment)
            {
                matchingIncrement.Image.color = _hoveredColour;
            }
            else
            {
                matchingIncrement.Image.color = index < _currentValue ? Color.white : _inactiveColour;
            }
        }
    }

    public void Unhover()
    {
        for (int index = 0; index < _increments.Length; index++)
        {
            StatBarIncrement matchingIncrement = _increments[index];
            
            matchingIncrement.Image.color = index < _currentValue ? Color.white : _inactiveColour;
        }
    }

    
}
