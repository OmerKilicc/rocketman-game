using UnityEngine;

/// <summary>
/// Controls the stick's bending animation based on touch input.
/// </summary>
[RequireComponent(typeof(Animator))]
public class StickInputController : MonoBehaviour
{
    public VoidGameEvent StickReleasedEvent;
    private Animator _animator;

    [Header("Settings")] [SerializeField] [Tooltip("How sensitive the stick is to touch movement")]
    private float _sensitivity = 0.01f;

    [SerializeField] [Tooltip("Maximum amount the stick can bend")] [Range(0f, 1f)]
    private float _maxBendAmount = 1f;

    private float _currentBendAmount;
    private bool _isReleasing;

    private static readonly int BendAmountParam = Animator.StringToHash("BendAmount");
    private static readonly int IsReleasedParam = Animator.StringToHash("IsReleased");

    #region Unity Lifecycle

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleRelease();
    }

    #endregion

    #region Event Handlers

    public void HandleTouchMove(Vector2 delta)
    {
        ResetReleaseState();
        UpdateBendAmount(delta);
    }

    public void HandleTouchCancelled(Vector2 position)
    {
        StartRelease();
    }

    #endregion

    #region Private Methods

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
        StickReleasedEvent.Raise();
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