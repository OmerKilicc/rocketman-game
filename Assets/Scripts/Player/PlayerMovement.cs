using System;
using SOs.Variables;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    private const float Gravity = -9.81f;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector2 _firstTouchPosition;
    private Vector2 _touchDelta;
    private Vector3 _position;
    private Vector3 _velocity;
    private Quaternion _targetRotation;
    
    private bool _isGliding = false;
    private int _isGlidingHash;
    private float _currentForwardVelocity;
    private bool _isDead = false;
    private bool _canMove = false;
    
    [SerializeField] private Transform childsTransform;
    [SerializeField] private FloatVariable stickBendAmount;

    [Header("Shot Settings")] 
    [SerializeField] private float forwardSpeedMultiplier;
    [SerializeField] private float upwardsSpeedMultiplier = 10f;
    [SerializeField] private float gravityMultiplierFalling = 1f;
    [Tooltip("Bigger degree means the player will go higher etc.")] 
    [SerializeField] private float shotAngle = 45f;
    [SerializeField] private float targetForwardVelocity = 100f;
    [SerializeField] private float dragForce = 10f;
    [SerializeField] private float terminalVelocity = 20f;

    [Header("Gliding Settings")] 
    [Tooltip("To make player go down more slowly than falling")] 
    [SerializeField] private float gravityMultiplierGliding = 0.5f;
    [Tooltip("Players turn sensitivity in X axis.")] 
    [SerializeField] private float turnSpeed = 50f; // Dönüş kuvveti
    [SerializeField] private float turnAmount = 1f;
    
    [Header("Rotation Settings")] 
    [SerializeField] private float rollSpeed = 720f;
    [Tooltip("Player's tilt towards the ground while in the air.")] 
    [SerializeField] private float currentRolAngle = 1f;
    [SerializeField] private float rollAmount = 10f;
    [SerializeField] private float maxRolAngle = 30f;
    
    #endregion

    #region Unity LifeCycle

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _isGlidingHash = Animator.StringToHash("isGliding");
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        // Air drag calculation
        if (_currentForwardVelocity > targetForwardVelocity)
        {
            _currentForwardVelocity = Mathf.Lerp(_currentForwardVelocity, targetForwardVelocity, dragForce * Time.deltaTime);    
        }
        
        if (_isGliding)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turnSpeed * Time.deltaTime);
            transform.position = ProjectileMotion(gravityMultiplierGliding);
            
            Quaternion newRotation = Quaternion.Euler(90,0,0);
            newRotation *= Quaternion.Euler(0, currentRolAngle, 0);
            
            // Child's rotation
            childsTransform.localRotation =
                Quaternion.Lerp(childsTransform.localRotation, newRotation, Time.deltaTime * 10);
        }
        
        else
        {
            transform.position = ProjectileMotion(gravityMultiplierFalling);
            
            // Child's x rotation
            childsTransform.RotateAround(childsTransform.position,childsTransform.right,rollSpeed * Time.deltaTime);
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
        _targetRotation = Quaternion.LookRotation(ForwardVelocityOnXZPlane(transform.forward),Vector3.up);
    }

    //OnTouchMove delta
    public void SetTouchDelta(Vector2 lastPosition)
    {
        _touchDelta = lastPosition - _firstTouchPosition; 
        currentRolAngle = Mathf.Clamp(_touchDelta.x * -rollAmount, -maxRolAngle,maxRolAngle);
        
        Rotate();
    }

    //onTouchCancelled
    public void ExitGliding(Vector2 position)
    {
        // Disable the animation
        _animator.SetBool(_isGlidingHash, false);
        
        // Stop the gliding movement and rotation
        _isGliding = false;
        //childsTransform.rotation = Quaternion.identity;
        _touchDelta = Vector2.zero;
    }

    #endregion

    #region Private Methods
    private void Rotate()
    {
        Quaternion yawRotate = Quaternion.Euler(0,_touchDelta.x * turnAmount,0);
        
        // Rotate target in yaw with calculated amount based on touchDelta
        _targetRotation *= yawRotate;
    }
    
    private Vector3 ForwardVelocityOnXZPlane(Vector3 forward)
    {
        forward.y = 0;
        return forward.normalized;
    }
    
    
    private Vector3 ProjectileMotion(float gravityMultiplier)
    {
        Vector3 forward = ForwardVelocityOnXZPlane(transform.forward);
        forward *= _currentForwardVelocity;

        _velocity.x = forward.x;
        _velocity.z = forward.z;
        
        _velocity.y += Gravity * gravityMultiplier * Time.deltaTime;
        _velocity.y = Mathf.Clamp(_velocity.y, -terminalVelocity, terminalVelocity);
        
        _position += _velocity * Time.deltaTime;
        
        return _position;
    }

    #endregion

    #region Public Methods

    public void DeathMovement()
    {
        if (_isDead)
        {
            return;
        }
        
        _isDead = true;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = _velocity;
    }

    public void AddForce(float jumpMultiplierFromPlatform)
    {
        _velocity.y = 1;
        _velocity.y *= jumpMultiplierFromPlatform;
    }
    
    public void Initialize()
    {
        _isDead = false;
        
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        
        transform.localRotation = Quaternion.identity;
        childsTransform.localRotation = Quaternion.identity;
        _velocity = Vector3.zero;

        _animator.SetBool(_isGlidingHash, false);
    }
    public void ShotFromStick()
    {
        float angleInRadians = shotAngle * Mathf.Deg2Rad;
        float throwForce = stickBendAmount.Value * forwardSpeedMultiplier;
        
        float forwardSpeed = throwForce * targetForwardVelocity  * Mathf.Cos(angleInRadians);
        float upwardSpeed = throwForce * upwardsSpeedMultiplier * Mathf.Sin(angleInRadians);

        _currentForwardVelocity = forwardSpeed;
        _velocity = new Vector3(0, upwardSpeed, forwardSpeed);
        
        _targetRotation = transform.rotation;
        _position = transform.position;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    #endregion
}