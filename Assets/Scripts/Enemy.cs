using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { EnemySlow, Enemy, EnemyFast }

    [SerializeField] private EnemyType type;

    public Transform Player;
    private NavMeshAgent navAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Enemy navigates to player and updates position every frame
    void Update()
    {
        navAgent.SetDestination(Player.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
