using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemyCount = 100;
    [SerializeField] private int enemyAmountPerSpawn = 5;
    [SerializeField] private float spawnDelayTime = 3.0f;

    [SerializeField] private Transform player;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EnemySpawnningWithDelay());
    }

    public IEnumerator EnemySpawnningWithDelay()
    {
        // Checks if the prefab is destroyed or not and adjusts spawn count based on that - null check to keep things from breaking
        while (true)
        {
            spawnedEnemies.RemoveAll(item => item == null);
            int missingEnemyCount = maxEnemyCount - spawnedEnemies.Count;

            // If there are more than 0 missing enemies from the 100 required, run this code
            if (missingEnemyCount > 0)
            {
                // Local values to ensure the correct amount of enemies spawn
                int enemyAmountToSpawn = Mathf.Min(enemyAmountPerSpawn, missingEnemyCount);

                // For loop to spawn the enemy prefabs until hitting the specified number
                for (int i = 0; i < enemyAmountToSpawn; i++)
                {
                    GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
                    Enemy enemyScript = newEnemy.GetComponent<Enemy>();
                    enemyScript.Player = player;

                    spawnedEnemies.Add(newEnemy);
                }
            }

            yield return new WaitForSeconds(spawnDelayTime);
        }
    }
}

