using UnityEngine;

[System.Serializable]
public class PlayerMovement : ICharacterMovement
{
    [Header("Speeds:")] 
    [SerializeField] private float _sprintSpeed = 2f; // This value is added to base speed value
    [SerializeField] private float _acceleration = 18f;
    [SerializeField] private float _decceleration = 22f;
    [SerializeField] private float _rotationSpeed = 12f;

    [Header("Ground:")] 
    [SerializeField] private GroundChecker _groundChecker = new GroundChecker();
    
    [Header("Root Motion")]
    [SerializeField] private bool _useRootMotion;
    [SerializeField, Range(0f,1f)] private float _rootMotionBlend = 1f;

    private PlayerContext _ctx;
    private Rigidbody _rb;
    private Transform _transform;

    private bool _isSprinting;

    private Transform _lockOnTarget;
    private Vector3 _desiredDir;
    private Vector3 _planarVelocity; // Axis X Z
    private Vector3 _rootDelta;
    
    public void SetContextAndInitialize(PlayerContext ctx)
    {
        _ctx = ctx;
        _rb = _ctx.Owner.GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _transform = _ctx.Transform;
    }
    
    public void HandleAllMovement()
    {
        _groundChecker.CheckGround(_transform);
        
        // Target speed
        Vector3 wishedVelocity = _desiredDir * TargetSpeed();
        
        // Acceleration & Decceleration in XZ plane
        var linearVelocity = _rb.linearVelocity;
        Vector3 currentPlanar = new Vector3(linearVelocity.x, 0f, linearVelocity.z);
        Vector3 delta = wishedVelocity - currentPlanar;

        float rate = (wishedVelocity.sqrMagnitude > 0.01f) ? _acceleration : _decceleration;

        Vector3 change = Vector3.ClampMagnitude(delta, rate * Time.fixedDeltaTime);
        _planarVelocity = currentPlanar + change;
        
        // Adapting to Slope
        if (_groundChecker.IsGrounded() && _groundChecker.OnWalkableSlope)
        {
            _planarVelocity = _groundChecker.ProjectOnGround(_planarVelocity);
        }
        
        // Blending RootMotion (if'ts active)
        if (_useRootMotion && _rootDelta != Vector3.zero)
        {
            Vector3 rootVelocity = new Vector3(_rootDelta.x, 0f, _rootDelta.z) / Time.fixedDeltaTime;
            _planarVelocity = Vector3.Lerp(_planarVelocity, rootVelocity, _rootMotionBlend);
        }
        
        var vy = _rb.linearVelocity.y;
        _rb.linearVelocity = new Vector3(_planarVelocity.x, vy, _planarVelocity.z);
        
        HandleRotation();
        
        _desiredDir = Vector3.zero;
        _rootDelta = Vector3.zero;
    }
    private float TargetSpeed()
    {
        float speed = _ctx.Stats.GetStatValue(EnumsNagu.StatType.Speed);
        if(_isSprinting)
        {
            return speed + _sprintSpeed;
        }

        return speed;
    }

    public void SetDesiredDirection(Vector3 dir) => _desiredDir = dir.sqrMagnitude > 0.001f ? dir.normalized : Vector3.zero;
    public void SetSprint(bool sprint) => _isSprinting = sprint;
    public void SetLockOn(Transform target) => _lockOnTarget = target;
    public void SetRootMotion(bool enabled) => _useRootMotion = enabled;


    #region Rotation Handlers

    private void HandleRotation()
    {
        if (_lockOnTarget != null)
        {
            Vector3 toTarget = _lockOnTarget.position - _transform.position;
            toTarget.y = 0f;
            if (toTarget.sqrMagnitude > 0.001f)
            {
                ApplyRotation(toTarget.normalized, _rotationSpeed);
                return;
            }
        }

        if (_desiredDir.sqrMagnitude > 0.0001f)
        {
            ApplyRotation(_desiredDir, _rotationSpeed);
        }
    }
    private void ApplyRotation(Vector3 dir, float speed)
    {
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        _rb.MoveRotation(Quaternion.Slerp(_transform.rotation, targetRot, speed * Time.fixedDeltaTime));
    }
    #endregion

    #region Root Motion
    public void AccumulateRootDelta(Vector3 delta) { _rootDelta += delta; }
    public void AccumulateRootRotation(Quaternion deltaRot) { /* si quieres rotación por clip */ }

    #endregion

}
