using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Handles touch input for the game using the new Input System.
/// Implements the singleton pattern for global access.
/// </summary>
public class TouchInputHandler : MonoBehaviour
{
    #region Singleton

    private static TouchInputHandler _instance;
    private static readonly object _lock = new object();

    /// <summary>
    /// Gets the singleton instance of the TouchInputHandler.
    /// </summary>
    public static TouchInputHandler Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance;
            }
        }
        private set
        {
            _instance = value;
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Triggered when a touch begins, providing the touch position.
    /// </summary>
    public event Action<Vector2> OnTouchStarted;

    /// <summary>
    /// Triggered when a touch moves, providing the touch delta.
    /// </summary>
    public event Action<Vector2> OnTouchMoved;

    /// <summary>
    /// Triggered when a touch ends, providing the final touch position.
    /// </summary>
    public event Action<Vector2> OnTouchEnded;

    /// <summary>
    /// Triggered when a touch is canceled.
    /// </summary>
    public event Action OnTouchCanceled;

    #endregion

    #region Properties

    /// <summary>
    /// Indicates whether the screen is currently being touched.
    /// </summary>
    public bool IsTouching { get; private set; }

    /// <summary>
    /// The current position of the touch in screen coordinates.
    /// </summary>
    public Vector2 TouchPosition { get; private set; }

    /// <summary>
    /// The change in touch position since the last frame.
    /// </summary>
    public Vector2 TouchDelta { get; private set; }

    #endregion

    #region Private Fields

    private PlayerInput _playerInput;
    private InputAction _touchPositionAction;
    private InputAction _touchPressAction;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        InitializeSingleton();
        InitializeInput();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
        
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #endregion

    #region Private Methods

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInput()
    {
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null)
        {
            _playerInput = gameObject.AddComponent<PlayerInput>();
            Debug.LogWarning($"[{nameof(TouchInputHandler)}] PlayerInput component was missing and has been added.");
        }

        _touchPositionAction = _playerInput.actions["TouchPosition"];
        _touchPressAction = _playerInput.actions["TouchPress"];

        if (_touchPositionAction == null || _touchPressAction == null)
        {
            Debug.LogError($"[{nameof(TouchInputHandler)}] Required input actions are missing!");
            return;
        }

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _touchPressAction.started += OnTouchStart;
        _touchPressAction.canceled += OnTouchCancel;
        _touchPositionAction.performed += OnTouchMove;
    }

    private void UnsubscribeFromEvents()
    {
        if (_touchPressAction != null)
        {
            _touchPressAction.started -= OnTouchStart;
            _touchPressAction.canceled -= OnTouchCancel;
        }

        if (_touchPositionAction != null)
        {
            _touchPositionAction.performed -= OnTouchMove;
        }
    }

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        IsTouching = true;
        TouchPosition = _touchPositionAction?.ReadValue<Vector2>() ?? Vector2.zero;
        TouchDelta = Vector2.zero;
        OnTouchStarted?.Invoke(TouchPosition);
    }

    private void OnTouchMove(InputAction.CallbackContext context)
    {
        if (!IsTouching) return;

        Vector2 newPosition = _touchPositionAction.ReadValue<Vector2>();
        TouchDelta = newPosition - TouchPosition;
        TouchPosition = newPosition;
        OnTouchMoved?.Invoke(TouchDelta);
    }

    private void OnTouchCancel(InputAction.CallbackContext context)
    {
        if (!IsTouching) return;

        IsTouching = false;
        TouchDelta = Vector2.zero;
        OnTouchEnded?.Invoke(TouchPosition);
        OnTouchCanceled?.Invoke();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Checks if the current touch position is within a specified screen area.
    /// </summary>
    /// <param name="area">The screen area to check against.</param>
    /// <returns>True if touching within the specified area, false otherwise.</returns>
    public bool IsTouchInArea(Rect area)
    {
        if (area == default)
        {
            Debug.LogWarning($"[{nameof(TouchInputHandler)}] Checking touch against an invalid area.");
            return false;
        }

        return IsTouching && area.Contains(TouchPosition);
    }

    #endregion
} 