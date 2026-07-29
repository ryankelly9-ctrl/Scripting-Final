using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { EnemySlow, Enemy, EnemyFast }

    [SerializeField] private EnemyType type;

    [SerializeField] private Transform player;
    private NavMeshAgent navAgent;

    private EnemySpawner spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Enemy navigates to player and updates position every frame
    void Update()
    {
        navAgent.SetDestination(player.position);
    }
}
