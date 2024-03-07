using TMPro;
using UnityEngine;

namespace Zoomies
{
    public class InstructionTextPulse : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _textComponent;
    
        [Header("Settings")]
        [SerializeField] private float _cycleDuration = 1f;
        [SerializeField] private float _fontSizeOffset = 0.5f;

        private float _baseFontSize;
    
        private void Awake()
        {
            _baseFontSize = _textComponent.fontSize;
        }

        private void Update()
        {
            float pulse01 = Mathf.PingPong(Time.time, _cycleDuration);

            Color color = _textComponent.color;
            color.a = pulse01;
            _textComponent.color = color;
        
            _textComponent.fontSize = (pulse01 + _fontSizeOffset) * _baseFontSize;
        }
    }
}
