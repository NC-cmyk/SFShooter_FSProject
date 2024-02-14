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

    [Header("-- Spawner Stats --")]
    [Range(1, 50)] [SerializeField] int numToSpawn;
    [Range(0, 10)] [SerializeField] int timeBetweenSpawns;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    /* === PREFAB NOTES ===
     * should automatically come with the 3 enemy types, spawns them randomly
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numToSpawn && !isSpawning)
        {
            StartCoroutine(spawn());
        }

        // the attached objective would have a component that when true would allow the collectible to be collected
        //if(startSpawning && CombatManager.instance.activeSpawner == null){
        //    objective.SetActive(false);
        //    Debug.Log("box is shut down");
        //}
        // so if spawnerComplete from combat manager is true then objective = true -> do something that gives access to collectible
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

        int arrayPos = Random.Range(0, spawnPositions.Length);
        Instantiate(enemyToSpawn, spawnPositions[arrayPos].transform.position, spawnPositions[arrayPos].transform.rotation, spawnerTrkr.transform);

        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !startSpawning)
        {
            // && CombatManager.instance.activeSpawner == null
            //CombatManager.instance.activeSpawner = gameObject;
            //CombatManager.instance.spawnerComplete = false;
            spawnerTrkr.GetComponent<SpawnerTracker>().updateEnemyCount(spawnCount);
            startSpawning = true;
        }
    }
}
