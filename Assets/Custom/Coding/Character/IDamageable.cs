using UnityEngine;
public interface IDamageable
{
    public int Health { get;set;}
    public void TakeDamages(int damages);
    public bool IsDeath();
    Transform GetTransform();
}
