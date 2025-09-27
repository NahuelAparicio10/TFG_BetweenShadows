using UnityEngine;

public class PlayerContext : CharacterContext
{
    public PlayerInputs Inputs { get; }
    public InputBuffer Buffer { get; }
    public ICharacterMovement Movement { get; }

    public PlayerContext(GameObject owner,
        PlayerInputs inputs,
        CharacterStats stats,
        CharacterHealthSystem health,
        ICharacterMovement movement, 
        CharacterAnimation animation) : base(owner, stats, health, animation)
    {
        Inputs = inputs;
        Buffer = Inputs?.Buffer ?? new InputBuffer(0.25f);
        Movement = movement;
    }
    
}
