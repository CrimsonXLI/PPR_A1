using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlingshotCat : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera _camera;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ZoomiesProgressBar _zoomiesProgressBar;
    [SerializeField] private VictoryBanner _victoryBanner;

    [Header("Physics")]
    [SerializeField] private float _forceMultiplier;
    [SerializeField] private float _idleDrag;
    [SerializeField] private float _idleAngularDrag;

    [Header("Time")]
    [SerializeField] private float _idleTimeScale = 0.025f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _catMeowSound;
    [SerializeField] private AudioClip _zoomiesCompleteSound;
    [SerializeField] private float _minimumPitch = 1.25f;
    [SerializeField] private float _maximumPitch = 1.75f;
    
    [Header("Zoomies")]
    [SerializeField] private float zoomiesDecayRate = 1f;
    [SerializeField] private float zoomiesGainRate = 1f;
    
    private MinigameState _minigameState;
    private Vector3 _dragStartPosition;
    private Vector3 _dragEndPosition;
    private float _zoomiesProgress;
    
    private enum MinigameState
    {
        Idle,
        Dragging,
        Flying,
        Complete
    }

    private Vector3 MouseWorldPositionXY
    {
        get
        {
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseWorldPositionXY = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);
            return mouseWorldPositionXY;
        }
    }

    private Vector3 DragVector
    {
        get
        {
            Vector3 dragVector = _dragEndPosition - _dragStartPosition;
            return dragVector;
        }
    }
    
    private void Awake()
    {
        _lineRenderer.positionCount = 2;
        
        _rigidbody.drag = _idleDrag;
        _rigidbody.angularDrag = _idleAngularDrag;
    }
    
    private void Update()
    {
        switch (_minigameState)
        {
            case MinigameState.Idle:
                UpdateIdle();
                break;
            
            case MinigameState.Dragging:
                UpdateDragging();
                break;
            
            case MinigameState.Flying:
                UpdateFlying();
                
                break;

            case MinigameState.Complete:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void UpdateIdle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EnterDraggingState();
            return;
        }
        
        LoseZoomiesProgress();
    }
    private void UpdateDragging()
    {
        if (DragVector != Vector3.zero && Input.GetMouseButtonUp(0))
        {
            EnterFlyingState();
            return;
        }
        
        _dragEndPosition = MouseWorldPositionXY;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + DragVector);

        LoseZoomiesProgress();
    }
    private void UpdateFlying()
    {
        float velocityMagnitude = _rigidbody.velocity.magnitude; 
        
        if (velocityMagnitude < 0.1f)
        {
            EnterIdleState();
            return;
        }
        
        GainZoomiesProgress(velocityMagnitude);
    }

    private void EnterDraggingState()
    {
        _minigameState = MinigameState.Dragging;
        
        _dragStartPosition = MouseWorldPositionXY;
        
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position);
        _lineRenderer.enabled = true;
    }
    private void EnterFlyingState()
    {
        _minigameState = MinigameState.Flying;
        
        _lineRenderer.enabled = false;
        
        _rigidbody.drag = 0f;
        _rigidbody.angularDrag = 0f;
        
        Vector3 forceVector = DragVector * _forceMultiplier;
        
        _rigidbody.AddForce(forceVector, ForceMode2D.Impulse);
        
        _audioSource.pitch = Random.Range(_minimumPitch, _maximumPitch);
        _audioSource.PlayOneShot(_catMeowSound);
        
        Time.timeScale = 1f;
    }
    private void EnterIdleState()
    {
        _minigameState = MinigameState.Idle;

        _rigidbody.drag = _idleDrag;
        _rigidbody.angularDrag = _idleAngularDrag;

        Time.timeScale = _idleTimeScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_minigameState != MinigameState.Flying) return;

        if (_rigidbody.velocity.y < 0) EnterIdleState();
    }
    
    private void GainZoomiesProgress(float velocityMagnitude)
    {
        _zoomiesProgress = Mathf.Clamp01(_zoomiesProgress + zoomiesGainRate * velocityMagnitude * Time.deltaTime);
        
        CheckZoomiesProgress();
        
        _zoomiesProgressBar.UpdateProgressBar(_zoomiesProgress);
    }
    private void CheckZoomiesProgress()
    {
        if (_zoomiesProgress >= 1f)
        {
            _audioSource.pitch = 1f;
            _audioSource.PlayOneShot(_zoomiesCompleteSound);
            
            _victoryBanner.DisplayVictoryBanner(); 
            
            _minigameState = MinigameState.Complete;
            return;
        }
    }
    private void LoseZoomiesProgress()
    {
        _zoomiesProgress = Mathf.Clamp01(_zoomiesProgress - zoomiesDecayRate * Time.deltaTime);
        
        _zoomiesProgressBar.UpdateProgressBar(_zoomiesProgress);
    }
    
    [Button]
    private void ModifyZoomiesProgress(float amount)
    {
        _zoomiesProgress = Mathf.Clamp01(_zoomiesProgress + amount);
        
        _zoomiesProgressBar.UpdateProgressBar(_zoomiesProgress);
    }
}
