

public class PlayerLocomotionState : BaseState<PlayerContext>
{
    public PlayerLocomotionState(StateMachine fsm, PlayerContext ctx) : base(fsm, ctx)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Tick(float dt)
    {
        
        if (_ctx.Buffer.TryConsume<DodgeCmd>(out var _))
        {
            _finiteStateMachine.Set(new PlayerDodgeState(_finiteStateMachine, _ctx));
            return;
        }

        if (_ctx.Buffer.TryConsume<LightAttack>(out var _) || _ctx.Buffer.TryConsume<HeavyAttackCmd>(out var _))
        {
            _finiteStateMachine.Set(new PlayerAttackState(_finiteStateMachine, _ctx));
            return;
        }
        
        _ctx.Movement.SetDesiredDirection(_ctx.Inputs.GetDirectionNormalized());
        // Sprint??
        
    }

    public override void FixedTick(float fixedDt)
    {
        _ctx.Movement.HandleAllMovement();
    }

    public override void HandleCommand(ICommand command)
    {
        base.HandleCommand(command);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
