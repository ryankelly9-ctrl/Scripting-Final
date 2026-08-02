using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _GameManager { get; private set; }

    public float StartingPlayerHealth = 5;
    public float CurrentPlayerHealth;
    public float StartingEnemyHealth = 1;
    public float CurrentEnemyHealth;

    public float PlayerWeaponDamage = 1;
    public float EnemyDamage = 1;

    public bool IsDead;
    private void Awake()
    {
        if (_GameManager && _GameManager != this)
        {
            Destroy(_GameManager);
        }
        _GameManager = this;
    }

    public void PlayerHitByEnemy()
    {
        CurrentPlayerHealth -= EnemyDamage;
        if(CurrentPlayerHealth <= 0)
        {
            IsDead = true;
        }
    }

    public void EnemyHitByPlayer()
    {
        CurrentEnemyHealth -= PlayerWeaponDamage;
        if(CurrentEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
