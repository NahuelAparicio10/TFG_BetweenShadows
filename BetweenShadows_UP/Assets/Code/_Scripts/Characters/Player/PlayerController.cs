using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : CharacterBase
{
    private PlayerContext _ctx;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerHUD _hud;
    
    protected override void Awake()
    {
        base.Awake();
        
        _ctx = new PlayerContext(gameObject, 
            GetComponent<PlayerInputs>(), 
            GetComponent<CharacterStats>(), 
            new CharacterHealthSystem(), 
            _movement,
            GetComponent<PlayerAnimation>());
        _movement.SetContextAndInitialize(_ctx);

        GetComponent<RootMotionRelay>().OnRootMotion += (dp, dr) =>
        {
            _movement.AccumulateRootDelta(dp);
            _movement.AccumulateRootRotation(dr);
        };
        
        _ctx.Health.OnHealthChanged += _hud.UpdateHealthBar;
    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Set(new PlayerLocomotionState(_stateMachine, _ctx));
    }

}
