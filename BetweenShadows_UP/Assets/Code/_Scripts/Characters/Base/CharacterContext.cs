using UnityEngine;

public class CharacterContext
{
    public GameObject Owner { get; }
    public Transform Transform { get; }
    public CharacterStats Stats { get; }
    public CharacterHealthSystem Health { get; }
    public CharacterAnimation Animation { get; }

    public CharacterContext(GameObject owner, CharacterStats stats, CharacterHealthSystem health, CharacterAnimation animation)
    {
        Owner = owner;
        Transform = Owner.transform;
        Stats = stats;
        Health = health;
        Animation = animation;
    }

}
