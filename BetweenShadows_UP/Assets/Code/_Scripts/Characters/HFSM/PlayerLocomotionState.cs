

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
        base.Tick(dt);
    }

    public override void FixedTick(float fixedDt)
    {
        base.FixedTick(fixedDt);
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
