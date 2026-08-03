using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _GameManager { get; private set; }

    public float StartingPlayerHealth = 5;
    public float CurrentPlayerHealth;
    public float CurrentEnemyHealth;
    public int StartingKillCount = 0;
    public int CurrentKillCount;

    public float PlayerWeaponDamage = 1;
    public float EnemyDamage = 1;

    public bool IsDead;

    [SerializeField] private Enemy enemyScript;
    private void Awake()
    {
        if (_GameManager && _GameManager != this)
        {
            Destroy(_GameManager);
            return;
        }
        _GameManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CurrentKillCount = StartingKillCount;
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
            GameManager._GameManager.CurrentKillCount += enemyScript.KillValue;
        }
    }

    public IEnumerator DestroyDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}
