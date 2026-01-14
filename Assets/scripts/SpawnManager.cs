using System.Collections;
using UnityEngine;

/*
 * SpawnManager
 *
 * This script is responsible for spawning enemies at regular intervals
 */
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;    // The enemy prefab to spawn
    [SerializeField] private GameObject _container; // Parent object to keep the hierarchy organized

    private float _spawnDelay = 1.5f; // Time between enemy spawns

    private void Start()
    {
        // Start spawning enemies repeatedly after a short initial delay
        InvokeRepeating("SpawnEnemy", 1.0f, _spawnDelay);
    }

    /// <summary>
    /// Instantiates an enemy at a random horizontal position and parents it to the container.
    /// </summary>

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-9.1f, 9.1f), transform.position.y, 0);
        GameObject enemyClone = Instantiate(_enemy, spawnPosition, Quaternion.identity);
        if (_container != null)
        {
            // Parent the enemy under the container to keep the hierarchy tidy
            enemyClone.transform.parent = _container.transform;
        }
    }

    /// <summary>
    /// Stops the repeated spawning of enemies.  Called by the GameManager when the player dies.
    /// </summary>
    public void StopSpawning()
    {
        CancelInvoke("SpawnEnemy");
        // Optionally destroy the container so existing enemies are removed from the hierarchy
        if (_container != null)
        {
            Destroy(_container);
        }
    }
}