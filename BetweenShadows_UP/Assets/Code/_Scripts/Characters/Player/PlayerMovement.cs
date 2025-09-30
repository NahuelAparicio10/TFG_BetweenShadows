using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerContext _ctx;
    private Rigidbody _rb;
    private GroundChecker _groundChecker;
    
    [Header("Top-down Movement")]
    [SerializeField] private float _moveSpeed = 4.5f;
    [SerializeField] private float _acceleration = 18f;
    [SerializeField] private float _deceleration = 22f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _attackRotationSpeed = 6f; 

    [Header("Sprint")]
    [SerializeField] private float _sprintBonus = 2f; 
    
    private Vector3 _planarVel;          // XZ
    private Vector3 _desiredDir;         // XZ normalizada (o cero)
    private bool _isSprinting;
    public void SetDesiredDirection(Vector3 dir) => _desiredDir = (dir.sqrMagnitude > 0.0001f) ? Vector3.ClampMagnitude(dir, 1f) : Vector3.zero;
    public void SetSprint(bool v) => _isSprinting = v;
    public bool IsSprinting => _isSprinting;
    public bool IsLockedOn => _ctx != null && _ctx.LockOnSystem != null && _ctx.LockOnSystem.IsLockedOn;
    public float RunSpeed => _ctx != null ? _ctx.Stats.GetStatValue(Enums.StatType.Speed) : _moveSpeed;
    public float SprintSpeed => RunSpeed + _sprintBonus;
    public float CurrentPlanarSpeed => new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.z).magnitude;
    public bool IsGrounded() => _groundChecker != null && _groundChecker.IsGrounded();
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    public void Initialize(PlayerContext ctx)
    {
        _ctx = ctx;
    }

    public void HandleAllMovement()
    {
        _groundChecker.CheckGround(transform);
        
        if (_ctx.Animation.IsInteracting)
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        }
        else
        {
            float baseSpeed = _isSprinting ? SprintSpeed : RunSpeed;
            float targetSpeed = baseSpeed * _desiredDir.magnitude;
            Vector3 targetPlanar = _desiredDir * targetSpeed;

            _planarVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            bool speedingUp = targetPlanar.sqrMagnitude > _planarVel.sqrMagnitude;
            float accel = speedingUp ? _acceleration : _deceleration;

            _planarVel = Vector3.MoveTowards(_planarVel, targetPlanar, accel * Time.deltaTime);

            _rb.linearVelocity = new Vector3(_planarVel.x, 0f, _planarVel.z);
        }

        float rotSpeed = _ctx.Animation.IsInteracting ? _attackRotationSpeed : _rotationSpeed;
        
        if (_ctx.LockOnSystem.IsLockedOn)
        {
            RotateTowardsTarget(_rotationSpeed);
        }
        else
        {
            RotateTowardsDirection(_desiredDir, rotSpeed);
        }
    }

    #region Root Motion
    private void OnAnimatorMove()
    {
        // Root motion activo TODO el ataque
        if (!_ctx.Animation.IsInteracting) return;

        // Posición: usa deltaPosition ajustado si tienes suelos inclinados (si no, directo)
        Vector3 delta = _ctx.Animation.Animator.deltaPosition;
        delta = _groundChecker.GetSlopeAdjustedRootMotion(delta);

        _rb.MovePosition(_rb.position + new Vector3(delta.x, 0f, delta.z));

        // Rotación: aplica deltaRotation del clip
        Quaternion newRot = _rb.rotation * _ctx.Animation.Animator.deltaRotation;
        _rb.MoveRotation(newRot);
    }
    #endregion

    #region Direcciones y Rotación

    private void RotateTowardsTarget(float speed)
    {
       /* Vector3 dir = _ctx.LockOnSystem.GetDirectionToTargetNormalized(transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion target = Quaternion.LookRotation(dir);
        Quaternion smooth = Quaternion.Slerp(_rb.rotation, target, 1f - Mathf.Exp(-speed * Time.deltaTime));
        _rb.MoveRotation(smooth);*/
    }

    private void RotateTowardsDirection(Vector3 dir, float speed)
    {
        if (dir.sqrMagnitude < 0.0001f) return;
        Quaternion target = Quaternion.LookRotation(dir);
        Quaternion smooth = Quaternion.Slerp(_rb.rotation, target, 1f - Mathf.Exp(-speed * Time.deltaTime));
        _rb.MoveRotation(smooth);
    }
    #endregion
}
