using SOs.Variables;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Controls the stick's bending animation based on touch input.
/// </summary>
[RequireComponent(typeof(Animator))]
public class StickInputController : MonoBehaviour
{
    #region Variable
    
    public VoidGameEvent stickReleasedEvent;
    private Animator _animator;
    private Vector2 _firstTouchPosition;
    private Vector2 _touchDelta;
    
    [Header("Settings")] 
    [Tooltip("How sensitive the stick is to touch movement")]
    [SerializeField] private float sensitivity = 0.01f;
    
    [Tooltip("Maximum amount the stick can bend")] 
    [Range(0f, 1f)]
    [SerializeField] private float maxBendAmount = 1f;
    
    [SerializeField] private FloatVariable currentBendAmount;

    private static readonly int BendAmountParam = Animator.StringToHash("BendAmount");
    private static readonly int IsReleasedParam = Animator.StringToHash("IsReleased");
    

    #endregion
    
    #region Unity Lifecycle

    private void Start()
    {
        _animator = GetComponent<Animator>();
        currentBendAmount.SetValue(0);
    }

    private void Update()
    {
        _animator.SetFloat(BendAmountParam, currentBendAmount.Value);
    }

    #endregion

    #region Event Handler

    public void HandleTouchEnter(Vector2 position)
    {
        _firstTouchPosition = position;
    }
    public void HandleTouchMove(Vector2 lastTouchPosition)
    {
        _touchDelta = lastTouchPosition - _firstTouchPosition;
        //Debug.Log(_touchDelta);
        UpdateBendAmount(_touchDelta.x);
    }

    public void HandleTouchCancelled(Vector2 position)
    {
        StartRelease();
    }

    #endregion

    #region Private Methods
    
    private void StartRelease()
    {
        if (currentBendAmount.Value <= 0)
        {
            Debug.Log("Stick is dragged in wrong position");
            return;
        }
        _animator.SetBool(IsReleasedParam, true);
        stickReleasedEvent.Raise();
    }

    private void UpdateBendAmount(float touchDeltaX)
    {
        float bendDelta = -touchDeltaX * sensitivity;
        currentBendAmount.SetValue(Mathf.Clamp(bendDelta, 0, maxBendAmount));
    }

    #endregion
}