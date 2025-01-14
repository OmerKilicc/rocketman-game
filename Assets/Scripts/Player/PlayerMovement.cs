using SOs.Variables;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    private const float Gravity = -9.81f;

    private Animator _animator;
    private Vector2 _firstTouchPosition;
    private Vector2 _touchDelta;
    private Vector3 _position;
    private Vector3 _velocity;
    private Quaternion _glidingAngle;
    private bool _isGliding = false;
    private int _isGlidingHash;
    private Transform _childsTransform;

    [SerializeField] private FloatVariable stickBendAmount;

    [Header("Shot Settings")] [Tooltip("Initial shot speed multiplier")] [SerializeField]
    private float speedMultiplier;

    [SerializeField] private float gravityMultiplierFalling = 1f;

    [Tooltip("Bigger degree means the player will go higher etc.")] [SerializeField]
    private float shotAngle = 45f;

    [Header("Gliding Settings")] [Tooltip("To make player go down more slowly than falling")] [SerializeField]
    private float gravityMultiplierGliding = 0.5f;

    [Tooltip("Players turn sensitivity in Y axis.")] [SerializeField]
    private float glidingTurnSensitivityY = 1f;

    [Tooltip("Players turn sensitivity in Z axis.")] [SerializeField]
    private float glidingTurnSensitivityZ = 1f;

    [Tooltip("Players turn sensitivity in X axis.")] [SerializeField]
    private float turnSpeed = 50f; // Dönüş kuvveti

    [Header("Rotation Settings")] [SerializeField]
    private float rotateSpeed = 5f;

    [SerializeField] private float smoothRotationMultiplier = 1f;

    [Tooltip("Player's tilt towards the ground while in the air.")] [SerializeField]
    private float playerGlidingRotation;

    #endregion

    #region Unity LifeCycle

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _isGlidingHash = Animator.StringToHash("isGliding");
        _childsTransform = GetComponentInChildren<Transform>();
        Initialize();
    }

    private void FixedUpdate()
    {
        if (_isGliding)
        {
            _glidingAngle = Quaternion.Euler(playerGlidingRotation,
                -_touchDelta.x * glidingTurnSensitivityY, -_touchDelta.x * glidingTurnSensitivityZ);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, _glidingAngle,
                smoothRotationMultiplier * Time.deltaTime);

            transform.position = ProjectileMotion(gravityMultiplierGliding);
        }
        else
        {
            transform.position = ProjectileMotion(gravityMultiplierFalling);
            _childsTransform.Rotate(rotateSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    #endregion

    #region Event Handlers

    //OnTouchStart
    public void StartGliding(Vector2 position)
    {
        _firstTouchPosition = position;
        _isGliding = true;
        _animator.SetBool(_isGlidingHash, true);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.fixedDeltaTime);
    }

    //OnTouchMove delta
    public void SetTouchDelta(Vector2 lastPosition)
    {
        _touchDelta = lastPosition - _firstTouchPosition;
        Debug.Log(_touchDelta.normalized.x);
        _touchDelta.Normalize();
    }

    //OnTouchCancelled
    public void ExitGliding(Vector2 position)
    {
        // Disable the animation
        _animator.SetBool(_isGlidingHash, false);
        
        // Stop the gliding movement and rotation
        _isGliding = false;
        _childsTransform.rotation = Quaternion.Euler(0, 0, 0);
        _touchDelta = Vector2.zero;
    }

    #endregion

    #region Private Methods

    private Vector3 ProjectileMotion(float gravityMultiplier)
    {
        _velocity.y += Gravity * gravityMultiplier * Time.deltaTime;
        _velocity.x += _touchDelta.x * Time.deltaTime * turnSpeed;

        _velocity = new Vector3(_velocity.x, _velocity.y, _velocity.z);
        _position += _velocity * Time.deltaTime;
        return _position;
    }

    private void Initialize()
    {
        float angleInRadians = shotAngle * Mathf.Deg2Rad;

        float forwardSpeed = stickBendAmount.Value * speedMultiplier * Mathf.Cos(angleInRadians);
        float upwardSpeed = speedMultiplier * Mathf.Sin(angleInRadians);

        //Vector3 localVelocity = new Vector3(0,upwardSpeed,forwardSpeed); 
        //_velocity =  transform.TransformDirection(localVelocity);

        _velocity = new Vector3(0, upwardSpeed, forwardSpeed);
        _position = transform.position;
    }

    #endregion
}