using UnityEngine;

public class PlayerContext : CharacterContext
{
    public PlayerInputs Inputs { get; }
    public StaminaSystem Stamina { get; }
    public PlayerAnimation Animation { get; }
    
    public ComboController Combo { get; }
    public PlayerMovement Movement { get; }
    public EquipmentHandler PCEquipmentHandler { get; }
    public PlayerLockOnSystem LockOnSystem { get; }
    
    public PlayerContext(
        GameObject owner, 
        CharacterStats stats, 
        CharacterHealthSystem health,
        PlayerInputs inputs,
        StaminaSystem stamina,
        PlayerAnimation animation,
        ComboController combo,
        PlayerMovement movement,
        EquipmentHandler equipHandler,
        PlayerLockOnSystem lockon
        ) : base(owner, stats, health)
    {
        Inputs = inputs;
        Stamina = stamina;
        Animation = animation;
        Combo = combo;
        Movement = movement;
        PCEquipmentHandler = equipHandler;
        LockOnSystem = lockon;
    }
}
