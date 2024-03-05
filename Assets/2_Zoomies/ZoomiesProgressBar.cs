using UnityEngine;
using UnityEngine.UI;

public class ZoomiesProgressBar : MonoBehaviour
{
    [SerializeField] private Image _progressBarFill;
    
    private void Awake()
    {
        _progressBarFill.fillAmount = 0f;
    }
    
    public void UpdateProgressBar(float progress)
    {
        _progressBarFill.fillAmount = progress;
    }
}
