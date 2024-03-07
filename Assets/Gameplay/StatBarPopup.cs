using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class StatBarPopup : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _textComponent;
    
        [Header("General Settings")]
        [SerializeField] private float _lifeSpan;
        
        [Header("Movement Settings")]
        [SerializeField] private float _upMoveSpeed;
        
        [Header("Font Settings")]
        [SerializeField] private float _fontGrowSpeed;
        [SerializeField] private float _minimumFontSize;
        [SerializeField] private float _maximumFontSize;

        private float _elapsedTime;
        private bool _isDisplayed;

        [Button]
        public void Display(int valueDelta)
        {
            _elapsedTime = 0;
            _textComponent.transform.position = transform.position;
            _textComponent.fontSize = _minimumFontSize;
            _textComponent.enabled = true;

            switch (valueDelta)
            {
                case < 0:
                    _textComponent.text = $"{valueDelta}";
                    _textComponent.color = Color.red;
                    break;
                
                case 0:
                    _textComponent.text = "+0";
                    _textComponent.color = Color.yellow;
                    break;
                
                case > 0:
                    _textComponent.text = $"+{valueDelta}";
                    _textComponent.color = Color.green;
                    break;
            }
            
            _isDisplayed = true;
        }

        private void Update()
        {
            if (!_isDisplayed) return;
            
            _textComponent.transform.position += Vector3.up * (_upMoveSpeed * Time.deltaTime);
            _textComponent.fontSize = Mathf.Clamp(_textComponent.fontSize + _fontGrowSpeed * Time.deltaTime, _minimumFontSize, _maximumFontSize);
            
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _lifeSpan) Hide();
        }

        private void Hide()
        {
            _elapsedTime = 0;
            _textComponent.transform.position = transform.position;
            _textComponent.enabled = false;
            
            _isDisplayed = false;
        }
    }
}
