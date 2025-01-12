using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Animator _animator;
    private Vector3 _position;
    private Vector3 velocity;
    private const float Gravity = 9.81f;
    private float _startTime;
    private float _elapsedTime;
    
    [Header("Falling Settings")] 
    [Tooltip("Initial shot speed")] 
    [SerializeField] private float speed;
    [SerializeField] private float gravityMultiplierFalling = 1f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float shotAngle = 45f;

    [Header("Gliding Settings")] 
    [Tooltip("Initial shot speed")] 
    [SerializeField] private float glidingSpeed = 3f;
    [SerializeField] private float gravityMultiplierGliding = 0.5f;

    private bool _isGliding = false;
    private int _isGlidingHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _isGlidingHash = Animator.StringToHash("isGliding");

        Initalize();
        _startTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (_isGliding)
            transform.position = ProjectileMotion(gravityMultiplierGliding);

        else
        {
            transform.position = ProjectileMotion(gravityMultiplierFalling);
            transform.rotation = Quaternion.Euler(rotateSpeed * Time.deltaTime, 0, 0);
        }
    }

    #region StateMethods

    //OnTouchStart
    public void StartGliding()
    {
        _isGliding = true;
        _animator.SetBool(_isGlidingHash, true);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    //OnTouchMove delta yollayacak
    // belki bunu burda ayrı olarak alabilirim
    public void HandleGliding(Vector2 touchDelta)
    {
        // TODO: Make it move side to side
        /*
        float delta = Mathf.Clamp(touchDelta.x,-1,1);
        transform.position = new Vector3(delta * _glidingSpeed,transform.position.y,transform.position.z);
        */
    }

    //OnTouchCancelled
    public void ExitGliding()
    {
        _isGliding = false;
        _animator.SetBool(_isGlidingHash, false);
    }

    #endregion
    
    private Vector3 ProjectileMotion(float gravityMultiplier)
    {
        _elapsedTime = Time.time - _startTime;
        
        velocity.y -= Gravity * gravityMultiplier * Time.deltaTime;
        _position += velocity * Time.deltaTime;

        return _position;
    }

    private float Initalize()
    {
        float angleInRadians = shotAngle * Mathf.Deg2Rad;
        
        float velocityZ = speed * Mathf.Cos(angleInRadians);
        float velocityY = speed * Mathf.Sin(angleInRadians);
        
        velocity =  new Vector3(0, velocityY, velocityZ);
        _position = transform.position;

        return velocityZ;
    }
}