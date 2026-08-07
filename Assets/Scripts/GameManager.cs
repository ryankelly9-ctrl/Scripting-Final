using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    public float DeathTime = 5.0f;

    [Header ("Bools")]
    public bool IsDead;
    public bool IsPaused = false;

    [Header ("Interface")]
    public GameObject pauseMenuInterface;
    public TextMeshProUGUI KillCountText;
    [SerializeField] private UnityEngine.UI.Image HealthOrb;
    [SerializeField] private UnityEngine.UIElements.Slider loadingMeter;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private const float minimumLoadTime = 1.5f;

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
        
        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }
    }

    private void Start()
    {
        CurrentKillCount = StartingKillCount;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }

        KillCountText.text = "Killed: " + CurrentKillCount;
    }

    // Player interactions

    public void PlayerHitByEnemy()
    {
        if (IsDead)
        {
            return;
        }

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
            TitleSceneLoadDelay(DeathTime);
        }
        HealthOrb.fillAmount = CurrentPlayerHealth / StartingPlayerHealth;
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

    public IEnumerator TitleSceneLoadDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadSceneAsync("Title");
    }

 //   private IEnumerator SceneLoading(string sceneName)
 //   {
 //       if (loadingCanvas != null)
//        {
 //           loadingCanvas.SetActive(true);
 //       }
 //       AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
 //       operation.allowSceneActivation = false;
 //      float timer = 0f;
 //
 //      while (!operation.isDone)
 //      {
 //          timer += Time.deltaTime;
 //          if (operation.progress >= 0.9f)
 //          {
 //             // if (timer >= MinimumLoadTime)
 //          }
 //      }
 //  }

    void LoadNextArea()
    {
        int nextAreaIndex = SceneManager.GetActiveScene().buildIndex + 1;
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

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            pauseMenuInterface.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuInterface.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
