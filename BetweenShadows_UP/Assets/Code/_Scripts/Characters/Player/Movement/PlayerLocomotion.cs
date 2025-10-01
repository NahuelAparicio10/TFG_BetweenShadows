using UnityEngine;

[System.Serializable]
public class PlayerLocomotion
{
      [SerializeField] private float _intentCoyote = 0.12f;    // intention seconds before not moving joystick
    [SerializeField] private float _turnResponsiveness = 16f; // s^-1 Slerp direction
    [SerializeField] private float _throttleResponsiveness = 10f; // s^-1 Lerp intensity
    private const float WALK_THRESHOLD = 0.30f; // has to be equal as the animator

    private Vector3 _smoothedAim = Vector3.forward; 
    private float _smoothedThrottle = 0f; // 0..1
    private Vector3 _lastIntentDir = Vector3.forward;
    private float _lastIntentTime = -999f;
    
    private PlayerContext _ctx;
    private PlayerMovement _movement;
    
    public void Initialize(PlayerContext ctx)
    {
        _ctx = ctx;
        _movement = (PlayerMovement)_ctx.Movement;
    }
    
    public void UpdateLocomotion()
    {
          // Input Movement To Animation bla bla gei
            Vector3 raw = _ctx.Inputs.GetDirectionNormalized(); 
            bool hasIntent;
            Vector3 targetDir;
            float targetThrottle;

            if (_ctx.Inputs.HasRaw())
            {
                _lastIntentDir = raw;
                _lastIntentTime = Time.time;
                hasIntent = true;
                targetDir = raw;
                targetThrottle = Mathf.Clamp01(Mathf.Sqrt(_ctx.Inputs.RawMag2())); // stick radius [0..1]
            }
            else if (Time.time - _lastIntentTime <= _intentCoyote)
            {
                // Intent hold: avoids a stop when the stick crosses through zero
                hasIntent = true;
                targetDir = _lastIntentDir;
                targetThrottle = 0f; // we don't force idle (avoid sudden idle; physical velocity keeps it moving)
            }
            else
            {
                hasIntent = false;
                targetDir = Vector3.zero;
                targetThrottle = 0f;
            }

            // Smooth orientation and throttle
            if (hasIntent)
            {
                Vector3 aimTarget = targetDir.sqrMagnitude > 0.0001f ? targetDir.normalized : _smoothedAim;
                _smoothedAim = (_smoothedAim == Vector3.zero)
                    ? aimTarget
                    : Vector3.Slerp(_smoothedAim, aimTarget, Mathf.Clamp01(_turnResponsiveness * Time.deltaTime));

                _smoothedThrottle = Mathf.Lerp(_smoothedThrottle, targetThrottle, Mathf.Clamp01(_throttleResponsiveness * Time.deltaTime));
            }
            else
            {
                _smoothedThrottle = Mathf.Lerp(_smoothedThrottle, 0f, Mathf.Clamp01(_throttleResponsiveness * Time.deltaTime));
                if (_smoothedThrottle < 0.01f) _smoothedAim = Vector3.zero;
            }

            // Deliver desired direction with magnitude (true analog)
            Vector3 desired = (_smoothedAim == Vector3.zero) ? Vector3.zero : _smoothedAim * _smoothedThrottle;
            _ctx.Movement.SetDesiredDirection(desired); // PlayerMovement now does NOT normalize; it keeps 0..1 magnitude

            var pm = (PlayerMovement)_ctx.Movement;

            // Animation: InputX/Y for lock-on (local space with intensity)
            if (_ctx.LockOnSystem.IsLockedOn && desired.sqrMagnitude > 0.0001f)
            {
                Vector3 local = _ctx.Transform.InverseTransformDirection(_smoothedAim) * _smoothedThrottle;
                _ctx.Animation.SetInputValuesDamped(new Vector2(local.x, local.z), Time.deltaTime);
            }
            else
            {
                _ctx.Animation.SetInputValuesDamped(Vector2.zero, Time.deltaTime);
            }

            //  Animation: "speed" mapped to your Blend Tree (0..0.5 walk, 0.5..1 run, 1..1.5 sprint)
            float speedParamFromVel = ComputeSpeedParamFromVelocity(pm);
            float speedParamFromStick = ComputeSpeedParamFromThrottle(pm.IsSprinting, _smoothedThrottle);

            // Avoid dropping to Idle when turning: use the max of intent and actual velocity
            float speedTarget = Mathf.Max(speedParamFromVel, speedParamFromStick);

            _ctx.Animation.SetSpeedDamped(speedTarget, Time.deltaTime);  
    }
    
    // Use physical (rigidbody) velocity to stabilize the animation (it won't drop to 0 when turning)
    private static float ComputeSpeedParamFromVelocity(PlayerMovement pm)
    {
        float v = pm.CurrentPlanarSpeed;
        float run = pm.RunSpeed;
        float sprint = pm.SprintSpeed;

        if (pm.IsSprinting)
        {
            // 1..1.5 based on the percentage between run and sprint speed
            float t = Mathf.InverseLerp(run, sprint, v);
            return Mathf.Clamp(1f + 0.5f * Mathf.Clamp01(t), 0f, 1.5f);
        }
        else
        {
            // 0..1 based on the percentage of run speed
            return Mathf.Clamp01(Mathf.InverseLerp(0f, run, v));
        }
    }

    // Use stick throttle for immediate response and walk/run/sprint control
    private static float ComputeSpeedParamFromThrottle(bool isSprinting, float throttle01)
    {
        if (throttle01 <= 0f) return 0f;

        // walk/run without sprint (respects 0..0.5 and 0.5..1)
        float baseNoSprint;
        if (throttle01 <= WALK_THRESHOLD)
        {
            baseNoSprint = Mathf.InverseLerp(0f, WALK_THRESHOLD, throttle01) * 0.5f; // 0..0.5
        }
        else
        {
            baseNoSprint = 0.5f + Mathf.InverseLerp(WALK_THRESHOLD, 1f, throttle01) * 0.5f; // 0.5..1
        }

        if (!isSprinting) return baseNoSprint;

        // sprint adds 0..0.5 up to 1.5, depending on how far the stick is pushed
        float sprintBlend = throttle01; 
        float withSprint = Mathf.Lerp(baseNoSprint, 1.5f, sprintBlend);
        return Mathf.Min(withSprint, 1.5f);
    }
}
