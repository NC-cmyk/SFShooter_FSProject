using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    /* === IMPORTANT ===
     * !!!! IF YOU'RE RECEIVING AN ERROR ABOUT A NULLREFERENCE !!!!
     * YOU PROBABLY PUT IN A "SPAWNER" GAMEOBJECT AND NOT THE "SPAWNER TRACKER" GAMEOBJECT
     * 
     * The list specifically needs the "Spawner Tracker" gameobject for it to work.
     * The "Spawner Tracker" gameobject should be the first child of the "Spawner" gameobject.
     * 
     * !!!! IF THE GATE DISAPPEARS UPON PLAYING THE GAME !!!!
     * You probably forgot to fill the Spawner Tracker list.
     */

    [Header("--- Gate Components ---")]
    [Header("DONT FORGET TO FILL SPAWNER TRACKER LIST")]
    [SerializeField] GameObject[] spawnerTrackers;

    bool isCounting;

    // Update is called once per frame
    void Update()
    {
        if (!isCounting)
        {
            checkCount();
        }
    }

    void checkCount()
    {
        isCounting = true;
        int scrap = 0;
        int listSize = spawnerTrackers.Length;

        for(int i = 0; i < listSize; i++)
        {
            bool collected = spawnerTrackers[i].GetComponent<SpawnerTracker>().getScrapCollected();
            if (collected)
            {
                scrap++;
            }
        }

        if(scrap == listSize)
        {
            Destroy(gameObject);
        }

        isCounting = false;
    }
}
