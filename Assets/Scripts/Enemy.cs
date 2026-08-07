using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { EnemySlow, Enemy, EnemyFast }
   
    public EnemyType FoeType;
    public int EnemyMaxHealth;
    public float EnemyMoveSpeed;
    public int KillValue;

    [Header ("Components")]
    public Transform Player;
    private NavMeshAgent navAgent;
    public Animator EnemyAnimator;

    public bool enemyIsRunning;
    [SerializeField] private float destroyDelaySeconds;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent.speed = EnemyMoveSpeed;
        GameManager._GameManager.CurrentEnemyHealth = EnemyMaxHealth;
        enemyIsRunning = true;
    }

    // Enemy navigates to player and updates position every frame
    void Update()
    {
        navAgent.SetDestination(Player.position);
        if (EnemyAnimator != null)
        {
            EnemyAnimator.SetBool("EnemyIsRunning", enemyIsRunning);
        }
    }

    // When the bullet hits the enemy, deal damage to the enemy and if the enemy dies add score
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameManager._GameManager.EnemyHitByPlayer();
            Destroy(gameObject);
        }
    }
}
