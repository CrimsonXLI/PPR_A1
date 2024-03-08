using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Zoomies
{
    public class ZoomiesMinigame : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private AudioSource _audioSource;

        [SerializeField] private Camera _camera;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private ZoomiesCat _cat;
        [SerializeField] private ZoomiesProgressBar _progressBar;
        [SerializeField] private ZoomiesVictoryBanner _victoryBanner;

        [Header("Settings")] [SerializeField] private float _forceMultiplier;
        [SerializeField] private float zoomiesDecayRate = 1f;
        [SerializeField] private float zoomiesGainRate = 1f;

        [Header("Audio")] [SerializeField] private AudioClip _catMeowSound;
        [SerializeField] private float _minimumPitch = 1.25f;
        [SerializeField] private float _maximumPitch = 1.75f;
        [SerializeField] private AudioClip _zoomiesCompleteSound;

        private float _progress;

        public event Action Finished;

        private Vector3 MouseWorldPositionXY
        {
            get
            {
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mouseWorldPositionXY = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);
                return mouseWorldPositionXY;
            }
        }

        private void OnEnable()
        {
            _lineRenderer.positionCount = 2;
            
            _progress = 0;
            _progressBar.SetProgress(_progress);
        }

        private void Update()
        {
            if (_progress >= 1) return;

            Vector3 currentCatPosition = _cat.transform.position;
            float currentSqrVelocity = _cat.Rigidbody.velocity.sqrMagnitude;

            if (Input.GetMouseButtonDown(0)) //START DRAGGING.
            {
                // _dragStartPosition = MouseWorldPositionXY;

                _lineRenderer.SetPosition(0, currentCatPosition);
                _lineRenderer.SetPosition(1, currentCatPosition);
                _lineRenderer.enabled = true;
            }
            else if (Input.GetMouseButton(0)) //CONTINUE DRAGGING.
            {
                // _dragEndPosition = MouseWorldPositionXY;

                _lineRenderer.SetPosition(0, currentCatPosition);
                _lineRenderer.SetPosition(1, MouseWorldPositionXY);
            }
            else if (Input.GetMouseButtonUp(0)) //STOP DRAGGING. START FLYING.
            {
                _lineRenderer.enabled = false;

                Vector3 dragVector = MouseWorldPositionXY - currentCatPosition;

                if (dragVector != Vector3.zero)
                {
                    _cat.Rigidbody.AddForce(dragVector * _forceMultiplier, ForceMode2D.Impulse);

                    _audioSource.pitch = Random.Range(_minimumPitch, _maximumPitch);
                    _audioSource.PlayOneShot(_catMeowSound);
                }
            }
            else if (_cat.Rigidbody.velocity.sqrMagnitude > 0.1f) //FLYING.
            {
                GainZoomiesProgress(currentSqrVelocity);
            }
            else //IDLE.
            {
                LoseZoomiesProgress();
            }
        }

        //------------------------------------------------------------------------------------------------------------------
        // Progress.
        //------------------------------------------------------------------------------------------------------------------

        private void GainZoomiesProgress(float velocitySqrMagnitude)
        {
            _progress = Mathf.Min(1, _progress + zoomiesGainRate * Time.deltaTime * velocitySqrMagnitude);
            _progressBar.SetProgress(_progress);

            CheckProgress();
        }

        private void LoseZoomiesProgress()
        {
            _progress = Mathf.Max(0, _progress - zoomiesDecayRate * Time.deltaTime);
            _progressBar.SetProgress(_progress);

            CheckProgress();
        }

        private void CheckProgress()
        {
            if (_progress >= 1)
            {
                _audioSource.pitch = 1f;
                _audioSource.PlayOneShot(_zoomiesCompleteSound);

                _victoryBanner.Display(OnVictoryBannerFinished);
            }
        }

        //------------------------------------------------------------------------------------------------------------------
        // Callbacks.
        //------------------------------------------------------------------------------------------------------------------

        private void OnVictoryBannerFinished()
        {
            Finished?.Invoke();
        }
    }
}