using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [Header("Player Input Events")]
    [SerializeField] private UnityEvent<Vector2> onTouchBegan;
    [SerializeField] private UnityEvent<Vector2> onTouchMoved;
    [SerializeField] private UnityEvent<Vector2> onTouchCancelled;
    
    [Header("Stick Input Events")]
    [SerializeField] private UnityEvent<Vector2> onStickBegan;
    [SerializeField] private UnityEvent<Vector2> onStickMoved;
    [SerializeField] private UnityEvent<Vector2> onStickEnded;

    [Header("Event To Start The Game")]
    [SerializeField] private VoidGameEvent onStartGame;

    private bool _isStickState = true;

    private void Start()
    {
        onStartGame.Raise();
        _isStickState = true;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (_isStickState)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        onStickBegan?.Invoke(touch.position);
                        break;
                    case TouchPhase.Moved:
                        onStickMoved?.Invoke(touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        onStickEnded?.Invoke(touch.position);
                        break;
                }
            }
            
            else
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        onTouchBegan?.Invoke(touch.position);
                        break;
                    case TouchPhase.Moved:
                        onTouchMoved?.Invoke(touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        onTouchCancelled?.Invoke(touch.position);
                        break;
                }
            }
        }
    }
    
    public void SetInputStateToStick(bool state)
    {
        _isStickState = state;
    }
}