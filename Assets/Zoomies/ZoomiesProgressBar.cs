using UnityEngine;
using UnityEngine.UI;

namespace Zoomies
{
    public class ZoomiesProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _progressBarFill;
    
        private void Awake()
        {
            _progressBarFill.fillAmount = 0f;
        }
    
        public void SetProgress(float progress)
        {
            _progressBarFill.fillAmount = progress;
        }
    }
}
