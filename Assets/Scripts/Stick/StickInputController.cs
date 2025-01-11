using UnityEngine;

/// <summary>
/// Controls the stick's bending animation based on touch input.
/// </summary>
[RequireComponent(typeof(Animator))]
public class StickInputController : MonoBehaviour
{
    private Animator _animator;

    [Header("Settings")]
    [SerializeField] 
    [Tooltip("How sensitive the stick is to touch movement")]
    private float _sensitivity = 0.01f;
    
    [SerializeField]
    [Tooltip("Maximum amount the stick can bend")]
    [Range(0f, 1f)]
    private float _maxBendAmount = 1f;
    
    private float _currentBendAmount;
    private bool _isReleasing;

    private static readonly int BendAmountParam = Animator.StringToHash("BendAmount");
    private static readonly int IsReleasedParam = Animator.StringToHash("IsReleased");

    #region Unity Lifecycle

    private void Start()
    {
        ValidateReferences();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void Update()
    {
        HandleRelease();
    }

    #endregion

    #region Event Handlers

    private void HandleTouchMove(Vector2 delta)
    {
        ResetReleaseState();
        UpdateBendAmount(delta);
    }

    private void HandleTouchEnd(Vector2 position)
    {
        StartRelease();
    }

    private void HandleTouchCancel()
    {
        StartRelease();
    }

    #endregion

    #region Private Methods

    private void ValidateReferences()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    private void SubscribeToEvents()
    {
        if (TouchInputHandler.Instance != null)
        {
            TouchInputHandler.Instance.OnTouchMoved += HandleTouchMove;
            TouchInputHandler.Instance.OnTouchEnded += HandleTouchEnd;
            TouchInputHandler.Instance.OnTouchCanceled += HandleTouchCancel;
        }
        else
        {
            Debug.LogWarning("TouchInputHandler instance not found!");
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (TouchInputHandler.Instance != null)
        {
            TouchInputHandler.Instance.OnTouchMoved -= HandleTouchMove;
            TouchInputHandler.Instance.OnTouchEnded -= HandleTouchEnd;
            TouchInputHandler.Instance.OnTouchCanceled -= HandleTouchCancel;
        }
    }

    private void ResetReleaseState()
    {
        _animator.SetBool(IsReleasedParam, false);
        _isReleasing = false;
    }

    private void UpdateBendAmount(Vector2 delta)
    {
        float bendDelta = -delta.x * _sensitivity;
        _currentBendAmount = Mathf.Clamp(_currentBendAmount + bendDelta, -_maxBendAmount, _maxBendAmount);
        UpdateBendAnimation();
    }

    private void StartRelease()
    {
        _animator.SetBool(IsReleasedParam, true);
        _isReleasing = true;
    }

    private void HandleRelease()
    {
        if (!_isReleasing || Mathf.Approximately(_currentBendAmount, 0f)) return;

        UpdateBendAnimation();
    }

    private void UpdateBendAnimation()
    {
        _animator.SetFloat(BendAmountParam, _currentBendAmount);
    }

    #endregion
} 