using System;
using UnityEngine;

public class CharacterHealthSystem : IDamageable, ICurable
{
    protected CharacterStats _stats;
    protected CharacterStat _healthStat;
    protected CharacterStat _defenseStat;
    protected float _currentHealth;

    public float MaxHealth => _healthStat.Value;
    
    #region Event Actions
    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnHeal;
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;
    #endregion

    public void Initialize(CharacterStats stats)
    {
        _stats = stats;
        _healthStat = _stats.GetStat(Enums.StatType.Health);
        _defenseStat = _stats.GetStat(Enums.StatType.Defense);
        _currentHealth = _healthStat.Value;
    }
    
    
    public virtual void TakeDamage(float damage, Enums.HitType type)
    {
        // Minimum 1 of dmg
        damage = Mathf.Max(damage - _defenseStat.Value, 1f); 
        
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        OnDamageTaken?.Invoke(damage);
        OnHealthChanged?.Invoke(_currentHealth, MaxHealth);

        if (!(_currentHealth <= 0)) return;
        
        Die();
    }

    public virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 direction, Enums.HitType type)
    {
        damage = Mathf.Max(damage - _defenseStat.Value, 1f); 
        
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        
        OnDamageTaken?.Invoke(damage);
        OnHealthChanged?.Invoke(_currentHealth, MaxHealth);
        
        if (!(_currentHealth <= 0)) return;
        Die();
    }
    
    public virtual void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, MaxHealth);
        OnHeal?.Invoke(amount);
        OnHealthChanged?.Invoke(_currentHealth, MaxHealth);
    }
    
    protected virtual void Die()
    {
        OnDeath?.Invoke();
    }

}
