using UnityEditor;
using UnityEngine;

[System.Serializable]
public class GroundChecker : MonoBehaviour
{
    [Header("Ground Detection Variables:")]
    [SerializeField] private LayerMask _groundLM;
    [SerializeField] private float _castRadius = 0.17f;
    [SerializeField] private float _castDistance = 1.1f;
    [SerializeField] private float _originOffset = 1.1f;
    
    [Header("Slope Check:")]
    [SerializeField] private float _maxSlopeAngle = 60f;
    
    private float _currentSlopeAngle;
    private bool _isGrounded;
    private RaycastHit _hit;

    public void CheckGround(Transform transform)
    {
        Vector3 origin = transform.position + Vector3.up * _originOffset;
     
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
    public Vector3 GetSlopeAdjustedRootMotion(Vector3 delta)
    {
        if (_hit.normal != Vector3.up)
        {
            return Vector3.ProjectOnPlane(delta, _hit.normal);
        }
        return delta;
    }
    public bool IsGrounded() => _isGrounded;
    public bool OnWalkableSlope => _currentSlopeAngle > 0f && _currentSlopeAngle < _maxSlopeAngle;
    public bool OnTooSteepSlope => _currentSlopeAngle >= _maxSlopeAngle;

    #region  Gizmos

    [Header("Gizmos")]
    [SerializeField] private bool _drawGizmos = true;
    [SerializeField] private Color _gizmoColorWalkable = new Color(0.2f, 0.9f, 0.2f, 0.9f);
    [SerializeField] private Color _gizmoColorTooSteep = new Color(1f, 0.6f, 0.1f, 0.9f);
    [SerializeField] private Color _gizmoColorNoGround = new Color(1f, 0.25f, 0.25f, 0.9f);
    [SerializeField] private Color _gizmoColorNormal = new Color(0.2f, 0.6f, 1f, 0.9f);

 private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;

        // Recalcular un "preview" en editor si no está en Play, usando el propio transform
        bool previewIsGrounded = _isGrounded;
        float previewSlope = _currentSlopeAngle;
        RaycastHit previewHit = _hit;

        Vector3 origin = transform.position + Vector3.up * _originOffset;
        Vector3 dir = Vector3.down;
        float dist = _castDistance;

        if (!Application.isPlaying)
        {
            if (Physics.SphereCast(origin, _castRadius, dir, out previewHit, dist, _groundLM, QueryTriggerInteraction.Ignore))
            {
                previewIsGrounded = true;
                previewSlope = Vector3.Angle(Vector3.up, previewHit.normal);
            }
            else
            {
                previewIsGrounded = false;
                previewSlope = 0f;
                previewHit = default;
            }
        }

        // Color según estado
        Color stateColor = previewIsGrounded
            ? (previewSlope >= _maxSlopeAngle ? _gizmoColorTooSteep : _gizmoColorWalkable)
            : _gizmoColorNoGround;

        // Línea/cuerpo del cast
        Gizmos.color = new Color(stateColor.r, stateColor.g, stateColor.b, 0.6f);
        Gizmos.DrawLine(origin, origin + dir * dist);
        // Esferas de inicio y fin del cast
        Gizmos.DrawWireSphere(origin, _castRadius);
        Gizmos.DrawWireSphere(origin + dir * dist, _castRadius);

        if (previewIsGrounded)
        {
            // Esfera en el punto de impacto
            Gizmos.color = stateColor;
            Gizmos.DrawSphere(previewHit.point, _castRadius * 0.35f);

            // Normal del suelo
            Gizmos.color = _gizmoColorNormal;
            Vector3 normalStart = previewHit.point + previewHit.normal * 0.02f;
            Gizmos.DrawLine(normalStart, normalStart + previewHit.normal * 0.6f);

            // Disco aproximado de contacto
            Gizmos.DrawWireSphere(previewHit.point, _castRadius * 0.6f);

            // Arco/etiqueta del ángulo de la pendiente
            #if UNITY_EDITOR
            Handles.color = stateColor;
            Vector3 up = Vector3.up;
            float radius = 0.45f;
            // Dibujar un pequeño arco entre "up" y la normal, en el plano perpendicular al eje de rotación
            Vector3 axis = Vector3.Cross(up, previewHit.normal);
            if (axis.sqrMagnitude > 0.0001f)
            {
                Quaternion rot = Quaternion.AngleAxis(-90f, axis.normalized);
                Vector3 fromDir = rot * up; // dirección de inicio para el arco
                Handles.DrawWireArc(previewHit.point, axis, fromDir, previewSlope, radius);
            }

            // Etiqueta con el ángulo
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = stateColor;
            Handles.Label(previewHit.point + Vector3.up * 0.1f, $"{previewSlope:0.#}°", style);
            #endif
        }
    }
    #endregion


}
