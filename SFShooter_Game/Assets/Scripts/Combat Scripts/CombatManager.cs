using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public GameObject activeSpawner;
    public bool spawnerComplete;

    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateEnemyCount(int num)
    {
        enemyCount += num;
        Debug.Log(enemyCount);

        if(activeSpawner != null && enemyCount < 1)
        {
            activeSpawner = null;
            spawnerComplete = true;
            Debug.Log("Spawner has been reset");
        }
    }
}
