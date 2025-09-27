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
            GetComponent<CharacterHealthSystem>(), 
            _movement,
            GetComponent<CharacterAnimation>());
        _movement.SetContextAndInitialize(_ctx);
    }

    protected override void Start()
    {
        base.Start();
        _stateMachine.Set(new PlayerLocomotionState(_stateMachine, _ctx));
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


}
