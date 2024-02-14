using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTracker : MonoBehaviour
{
    [Header("--- Spawner Tracker Components ---")]
    [SerializeField] Transform trackerObject;
    [SerializeField] GameObject container; // container that holds the scrap

    int enemyCount;
    bool countAssigned;

    // Start is called before the first frame update
    void Start()
    {
        countAssigned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (countAssigned)
        {
            enemyCount = trackerObject.childCount;
            if(enemyCount <= 0)
            {
                container.SetActive(false);
            }
        }
    }

    public void updateEnemyCount(int amount)
    {
        if (!countAssigned)
        {
            countAssigned = true;
            enemyCount = amount;
        }
    }
}
