using UnityEngine;

public class PlayerContext : CharacterContext
{
    public PlayerInputs Inputs { get; }
    public InputBuffer Buffer { get; }
    public ICharacterMovement Movement { get; }
    
    public PlayerAnimation Animation { get; }

    public PlayerContext(GameObject owner,
        PlayerInputs inputs,
        CharacterStats stats,
        CharacterHealthSystem health,
        ICharacterMovement movement, 
        PlayerAnimation animation) : base(owner, stats, health)
    {
        Inputs = inputs;
        Buffer = Inputs?.Buffer ?? new InputBuffer(0.25f);
        Movement = movement;
        Animation = animation;
    }
    
}
