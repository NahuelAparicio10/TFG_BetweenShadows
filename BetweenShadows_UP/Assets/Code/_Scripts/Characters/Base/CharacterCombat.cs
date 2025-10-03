using System;

[System.Serializable]
public class CharacterCombat
{
    protected WeaponHandler _weaponHandler;
    protected PlayerContext _context;
    public event Action<AttackData> OnAttackStarted;

    public virtual void Initialize(PlayerContext ctx, WeaponHandler weaponHandler)
    {
        _weaponHandler = weaponHandler;
        _context = ctx;
    }

    protected void InvokeAttack(AttackData atkData) => OnAttackStarted?.Invoke(atkData);
}
