using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("CHECK SCRIPT FOR PREFAB NOTES")]
    [Header("-- Spawner Components --")]
    [SerializeField] GameObject[] enemies; // spawners only really used to spawn enemies
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject spawnerTrkr; // spawner tracker
    [SerializeField] GameObject spawnSmoke;

    [Header("-- Spawner Stats --")]
    [Range(1, 50)] [SerializeField] int numToSpawn;
    [Range(0, 10)] [SerializeField] int timeBetweenSpawns;
    [SerializeField] bool randomSpawn;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    int spawnIndex;

    /* === PREFAB NOTES ===
     * should automatically come with 2 enemy types (melee and exploding), spawns them randomly
     * if you only want specific enemy types, just remove the ones not wanted
     * 
     * spawner prefab comes with 4 spawn positions default and these can be moved and rotated to suit environment
     * if you want more or less spawn positions, just add or remove
     * 
     * spawner prefab has a sphere collider for its trigger, the size and position of the trigger can be resized if desired
     * the collider can also be changed into a box collider, especially if the player walks through an entryway and a trigger is needed there
     * 
     * !!!! RANGED ENEMIES ARE NOT PART OF THE DEFAULT SPAWNER PREFAB !!!!
     * this is due to them needing a kill switch
     */

    private void Start()
    {
        if (!randomSpawn)
        {
            spawnIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numToSpawn && !isSpawning)
        {
            StartCoroutine(spawn());
            if(spawnCount == numToSpawn)
            {
                spawnerTrkr.GetComponent<SpawnerTracker>().updateEnemyCount(spawnCount);
            }
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        int enemyNdx = 0;

        // randomize enemy spawns for fun
        if (enemies.Length > 1)
        {
            enemyNdx = Random.Range(0, enemies.Length);
        }

        GameObject enemyToSpawn = enemies[enemyNdx];

        if (randomSpawn)
        {
            // choose a random position to spawn
            int arrayPos = Random.Range(0, spawnPositions.Length);
            Instantiate(spawnSmoke, spawnPositions[arrayPos].transform.position, spawnPositions[arrayPos].transform.rotation);
            Instantiate(enemyToSpawn, spawnPositions[arrayPos].transform.position, spawnPositions[arrayPos].transform.rotation, spawnerTrkr.transform);
        }
        else
        {
            // spawn enemies in order of the spawn positions, this is the default
            Instantiate(spawnSmoke, spawnPositions[spawnIndex].transform.position, spawnPositions[spawnIndex].transform.rotation);
            Instantiate(enemyToSpawn, spawnPositions[spawnIndex].transform.position, spawnPositions[spawnIndex].transform.rotation, spawnerTrkr.transform);

            if(spawnIndex + 1 != spawnPositions.Length)
            {
                // increase index if possible
                spawnIndex++;
            }
            else
            {
                // go back to 0 if not
                spawnIndex = 0;
            }
        }

        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !startSpawning)
        {
            startSpawning = true;
        }
    }
}
