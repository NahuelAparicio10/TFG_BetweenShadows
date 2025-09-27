using UnityEngine;

[System.Serializable]
public class GroundChecker
{
    [Header("Ground Detection Variables:")]
    [SerializeField] private LayerMask _groundLM;
    [SerializeField] private float _castRadius = 0.17f;
    [SerializeField] private float _castDistance = 1.1f;
    
    [Header("Slope Check:")]
    [SerializeField] private float _maxSlopeAngle = 60f;
    
    
    private float _currentSlopeAngle;
    private bool _isGrounded;
    private RaycastHit _hit;
    

    public void CheckGround(Transform transform)
    {
        Vector3 origin = transform.position + Vector3.up * 0.05f;
     
        if (Physics.SphereCast(origin, _castRadius, Vector3.down, out _hit, _castDistance, _groundLM))
        {
            _isGrounded = true;
            _currentSlopeAngle = Vector3.Angle(Vector3.up, _hit.normal);
        }
        else
        {
            _isGrounded = false;
            _hit = default;
        }
    }

    public Vector3 ProjectOnGround(Vector3 movement)
    {
        if (!_isGrounded || _hit.normal == Vector3.zero) return movement;
        return Vector3.ProjectOnPlane(movement, _hit.normal);
    }
    
    public bool IsGrounded() => _isGrounded;
    public bool OnWalkableSlope => _currentSlopeAngle > 0f && _currentSlopeAngle < _maxSlopeAngle;
    public bool OnTooSteepSlope => _currentSlopeAngle >= _maxSlopeAngle;
}
