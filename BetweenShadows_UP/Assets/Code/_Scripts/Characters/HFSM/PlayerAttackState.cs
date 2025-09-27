using UnityEngine;

public class PlayerAttackState : BaseState<PlayerContext>
{
    public PlayerAttackState(StateMachine fsm, PlayerContext ctx) : base(fsm, ctx)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("In Attack State");
        _ctx.Movement.SetRootMotion(true);
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
        _ctx.Movement.SetRootMotion(false);
    }
}
