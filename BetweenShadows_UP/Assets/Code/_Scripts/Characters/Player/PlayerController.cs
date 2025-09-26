using System;
using UnityEngine;

public class PlayerController : CharacterBase
{
    private PlayerContext _ctx;
    [SerializeField] private PlayerHUD _hud;
    private EnumsNagu.CharacterState _currentState;
    
    protected override void Awake()
    {
        base.Awake();
        
        _ctx = new PlayerContext(gameObject, 
            GetComponent<PlayerInputs>(), 
            GetComponent<CharacterStats>(), 
            GetComponent<CharacterHealthSystem>(), 
            new PlayerMovement());
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        _ctx.Movement.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }


}
