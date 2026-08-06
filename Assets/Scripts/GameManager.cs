using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _GameManager { get; private set; }

    [Header ("Variables KillCount/Health")]
    public float StartingPlayerHealth = 5;
    public float CurrentPlayerHealth;
    public float deadHealthAmount = 0f;
    public float CurrentEnemyHealth;
    public int StartingKillCount = 0;
    public int CurrentKillCount;
    private int lastKillCap = 0;
    private const int killsPerArea = 250;

    [Header ("Adjustibles")]
    public float PlayerWeaponDamage = 1;
    public float EnemyDamage = 1;

    public bool IsDead;

    [Header ("Other Scripts")]
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private PlayerController playerControllerScript;
    [SerializeField] private Enemy enenmyScript;
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
        if (playerControllerScript.PlayerAnimator != null)
        {
            playerControllerScript.PlayerAnimator.SetTrigger("IsHit");
        }
        if(CurrentPlayerHealth <= deadHealthAmount)
        {
            IsDead = true;
            if (playerControllerScript.PlayerAnimator != null)
            {
                playerControllerScript.PlayerAnimator.SetTrigger("IsDead");
            }
        }
    }

    public void EnemyHitByPlayer()
    {
        CurrentEnemyHealth -= PlayerWeaponDamage;
        if(CurrentEnemyHealth <= deadHealthAmount)
        {
            GameManager._GameManager.CurrentKillCount += enemyScript.KillValue;
            if (enemyScript.EnemyAnimator != null)
            {
                enemyScript.EnemyAnimator.SetTrigger("EnemyIsDead");
            }
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

    // Menu stuff, all meant for button presses within the title
    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
