using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 1.5f;

    private bool isSpawning = true;

    void Start()
    {
        if (enemies.Length == 0 || spawnPoints.Length == 0)
        {
            return;
        }
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
        }
    }
    public void ClearEnemy()
    {
 
        isSpawning = false;

        foreach (GameObject enemyObj in spawnedEnemies)
        {
            if (enemyObj != null)
            {
                var enemyScript = enemyObj.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    enemyScript.Die();
                }
                else
                {
                    Destroy(enemyObj);
                }
            }
        }
        spawnedEnemies.Clear();
    }
}