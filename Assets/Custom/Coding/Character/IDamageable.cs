using UnityEngine;
public interface IDamageable
{
    public float MaxHealth { get; set; }
    public float Health { get;set;}
    public void TakeDamages(float damages);
    public bool IsDeath();
    Transform GetTransform();
}
