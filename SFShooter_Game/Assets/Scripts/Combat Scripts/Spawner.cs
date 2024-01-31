using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [Header("-- Spawner Components --")]
    [SerializeField] GameObject[] enemies; // spawners only really used to spawn enemies
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject objective; // general spawners have objectives that the enemies "protect"

    [Header("-- Spawner Stats --")]
    [SerializeField] int numToSpawn;
    [SerializeField] int timeBetweenSpawns;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    // Start is called before the first frame update
    void Start()
    {
        // GameManager.instance.GameGoalUpdate(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numToSpawn && !isSpawning)
        {
            CombatManager.instance.updateEnemyCount(numToSpawn);
            StartCoroutine(spawn());
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        // randomize enemy spawns for fun
        int enemyNdx = Random.Range(0, enemies.Length);

        GameObject enemyToSpawn = enemies[enemyNdx];
        enemyToSpawn.gameObject.tag = "Spawner Enemy";

        int arrayPos = Random.Range(0, spawnPositions.Length);
        Instantiate(enemyToSpawn, spawnPositions[arrayPos].transform.position, spawnPositions[arrayPos].transform.rotation);

        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CombatManager.instance.activeSpawner == null)
        {
            CombatManager.instance.activeSpawner = gameObject;
            startSpawning = true;
        }
    }
}
