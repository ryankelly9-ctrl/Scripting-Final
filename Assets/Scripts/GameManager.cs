using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _GameManager { get; private set; }

    public float StartingPlayerHealth = 5;
    public float CurrentPlayerHealth;
    public float CurrentEnemyHealth;
    public int StartingKillCount = 0;
    public int CurrentKillCount;
    private int lastKillCap = 0;
    private const int killsPerArea = 250;

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
            if (CurrentKillCount / killsPerArea > lastKillCap)
            {
                lastKillCap = CurrentKillCount / killsPerArea;
                LoadNextArea();
            }
        }
    }

    public IEnumerator DestroyDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }

    void LoadNextArea()
    {
        int nextAreaIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextAreaIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextAreaIndex);
        }
        else
        {

            // transition text appear here
        }
    }
}
