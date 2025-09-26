using UnityEngine;
// -- Interface for Damageable Components such as PlayerHealth
public interface IDamageable
{
    void TakeDamage(float damage);
    void TakeDamage(float damage, Vector3 hitPoint, Vector3 direction);
}
