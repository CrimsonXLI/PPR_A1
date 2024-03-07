using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Zoomies
{
    public class ZoomiesVictoryBanner : MonoBehaviour
    {
        [SerializeField] private Image _bannerImage;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private float _minScale = 1f;
        [SerializeField] private float _maxScale = 2f;

        private Coroutine _bannerScalingCR;

        [Button]
        public void Display(Action onCompleteCallback)
        {
            StartScalingCoroutine(onCompleteCallback);
        }

        private void StartScalingCoroutine(Action onCompleteCallback)
        {
            if ( _bannerScalingCR != null )
            {
                StopCoroutine(_bannerScalingCR);
            }
        
            _bannerScalingCR = StartCoroutine(ScaleUpInSizeCR(onCompleteCallback));
        }

        private IEnumerator ScaleUpInSizeCR(Action onCompleteCallback)
        {
            _bannerImage.transform.localScale = new Vector3(_minScale, _minScale, 1);
        
            _bannerImage.enabled = true;
        
            float elapsed = 0;

            while (elapsed < _duration)
            {
                float scale = Mathf.Lerp(_minScale, _maxScale, elapsed / _duration);
            
                _bannerImage.transform.localScale = new Vector3(scale, scale, 1);
            
                elapsed += Time.deltaTime;
            
                yield return null;
            }
        
            _bannerImage.transform.localScale = new Vector3(_maxScale, _maxScale, 1);
        
            yield return new WaitForSeconds(1);
        
            _bannerImage.enabled = false;
        
            _bannerScalingCR = null;
            
            onCompleteCallback?.Invoke();
        }
    }
}
