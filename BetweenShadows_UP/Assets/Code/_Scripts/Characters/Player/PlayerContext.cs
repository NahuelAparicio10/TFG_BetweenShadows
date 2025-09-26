using UnityEngine;

public class PlayerContext : CharacterContext
{
    public PlayerInputs Inputs { get; }
    public InputBuffer Buffer { get; }
    public PlayerMovement Movement { get; }

    public PlayerContext(GameObject owner,
        PlayerInputs inputs,
        CharacterStats stats,
        CharacterHealthSystem health,
        PlayerMovement movement) : base(owner, stats, health)
    {
        Inputs = inputs;
        Buffer = Inputs?.Buffer ?? new InputBuffer(0.25f);
        Movement = movement;
    }
    
}
