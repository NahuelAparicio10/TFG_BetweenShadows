using System;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Enums.EquipSlot _weaponSlot = Enums.EquipSlot.Weapon;
    
    private PlayerContext _ctx;
    private EquipmentHandler _equipmentHandler;
    private RuntimeAnimatorController _baseController;
    public WeaponData CurrentWeapon { get; private set; }
    public WeaponData baseWeapon; // This weapons is hands (you can't be without weapon or gg)
    public void Initialize(PlayerContext ctx)
    {
        _ctx = ctx;
        _equipmentHandler = _ctx.PCEquipmentHandler;
        _baseController = _ctx.Animation.Animator.runtimeAnimatorController;

        _equipmentHandler.OnEquipped += OnEquipped;
        _equipmentHandler.OnUnequipped += OnUnequipped;

        _equipmentHandler.Equip(baseWeapon);
    }
    private void SetPlayerContext(PlayerContext ctx) => _ctx = ctx;

    private void OnEquipped(Enums.EquipSlot slot, EquipableItemData item)
    {
        if(slot != _weaponSlot) return;
        CurrentWeapon = item as WeaponData;
        _ctx.Combo.currentWeapon = CurrentWeapon;
        
        if (CurrentWeapon.animatorOverride == null)
        {
            _ctx.Animation.Animator.runtimeAnimatorController = _baseController;
            return;
        }
        
        _ctx.Animation.Animator.runtimeAnimatorController = CurrentWeapon.animatorOverride;
    }
    private void OnUnequipped(Enums.EquipSlot slot, EquipableItemData item)
    {
        if(slot != _weaponSlot) return;
        CurrentWeapon = baseWeapon;
        _ctx.Combo.currentWeapon = baseWeapon;
        _ctx.Animation.Animator.runtimeAnimatorController = _baseController;
    }
    
    public bool HasWeapon() => CurrentWeapon != baseWeapon;
    
    private void OnDestroy()
    {
        _equipmentHandler.OnEquipped -= OnEquipped;
        _equipmentHandler.OnUnequipped -= OnUnequipped;
    }
}
