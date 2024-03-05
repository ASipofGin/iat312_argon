using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float spawnInterval = 3f; // Time interval between spawns
    public int maxEnemies = 10; // Maximum number of enemies to spawn
    private int currentEnemies = 0; // Current number of enemies spawned
    private Transform spawnPoint; // Reference to the spawn point

    void Start()
    {
        spawnPoint = transform; // Get the spawn point's transform
        StartCoroutine(SpawnEnemyRoutine()); // Start spawning enemies
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Wait for spawnInterval before spawning another enemy
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); // Spawn enemy at the spawn point's position and rotation
        currentEnemies++; // Increment the current enemy count
    }
}
