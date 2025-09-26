public class ComboSystem
{
    private int _lightIndex = 0;
    private int _heavyIndex = 0;

    public AttackData GetNextLightAttack(WeaponData weapon)
    {
        return weapon.lightCombo[_lightIndex++ % weapon.lightCombo.Length];
    }

    public AttackData GetNextHeavyAttack(WeaponData weapon)
    {
        return weapon.heavyCombo[_heavyIndex++ % weapon.heavyCombo.Length];
    }

    public void Reset()
    {
        _lightIndex = 0;
        _heavyIndex = 0;
    }
}
