using UnityEngine;

public class PlayerModel: IDamagable
{
    public float Speed { get; private set; }
    public float Health { get; private set; }
    
    public PlayerModel(float speed, float health)
    {
        Speed = speed;
        Health = health;
    }
    
    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log($"Player took {damage} damage! Health remaining: {Health}");
    }
}