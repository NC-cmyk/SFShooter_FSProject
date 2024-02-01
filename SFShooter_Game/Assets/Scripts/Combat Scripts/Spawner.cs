using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [Header("CHECK SCRIPT FOR PREFAB NOTES")]
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

    public bool Complete;

    /* === PREFAB NOTES ===
     * should automatically come with the 3 enemy types, spawns them randomly
     * if you only want specific enemy types, just remove the ones not wanted
     * 
     * spawner prefab comes with 4 spawn positions default and these can be moved and rotated to suit environment
     * if you want more or less spawn positions, just add or remove
     * 
     * spawner prefab has a sphere collider for its trigger, the size and position of the trigger can be resized if desired
     */

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
            StartCoroutine(spawn());
        }

        // the attached objective would have a component that when true would allow the collectible to be collected
        if(startSpawning && CombatManager.instance.activeSpawner == null){
            objective.SetActive(false);
            Debug.Log("box is shut down");
        }
        // so if spawnerComplete from combat manager is true then objective = true -> do something that gives access to collectible
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
        if (other.CompareTag("Player") && CombatManager.instance.activeSpawner == null && !startSpawning)
        {
            CombatManager.instance.activeSpawner = gameObject;
            CombatManager.instance.spawnerComplete = false;
            CombatManager.instance.updateEnemyCount(numToSpawn);
            startSpawning = true;
        }
    }
}
