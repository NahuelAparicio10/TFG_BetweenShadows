using System;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private EnumsNagu.EquipSlot _weaponSlot = EnumsNagu.EquipSlot.Weapon;
    
    private EquipmentHandler _equipmentHandler;
    private Animator _animator;
    private RuntimeAnimatorController _baseController;
    
    public WeaponData CurrentWeapon { get; private set; }

    public WeaponData weapon;
    private void Awake()
    {
        _equipmentHandler = GetComponent<EquipmentHandler>();
        _animator = GetComponent<Animator>();
        _baseController = _animator.runtimeAnimatorController;
        _equipmentHandler.OnEquipped += OnEquipped;
        _equipmentHandler.OnUnequipped += OnUnequipped;

    }

    private void Start()
    {
        _equipmentHandler.Equip(weapon);

    }

    private void OnEquipped(EnumsNagu.EquipSlot slot, EquipableItemData item)
    {
        if(slot != _weaponSlot) return;
        CurrentWeapon = item as WeaponData;
       // _animator.runtimeAnimatorController = CurrentWeapon.animatorOverride;
    }
    private void OnUnequipped(EnumsNagu.EquipSlot slot, EquipableItemData item)
    {
        if(slot != _weaponSlot) return;
        CurrentWeapon = null;
        _animator.runtimeAnimatorController = _baseController;

    }
    
    public bool HasWeapon() => CurrentWeapon != null;
}
