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

    [Header("--- Gate Stats ---")]
    [Range(3, 5)] [SerializeField] int gateSpeed; // how fast the gate lowers or rises

    [Header("--- Gate Components ---")]
    [Header("DONT FORGET TO FILL SPAWNER TRACKER LIST")]
    [SerializeField] GameObject[] spawnerTrackers;
    [SerializeField] Renderer model;
    [SerializeField] Material openMat;

    [Header("--- Gate Audio ---")]
    [SerializeField] AudioSource audSource;
    [SerializeField] AudioClip gateClose;
    [SerializeField] AudioClip gateOpen;

    bool isCounting;
    bool lowerDown;
    bool spawnerDone;
    float ogY;

    private void Start()
    {
        ogY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCounting && !spawnerDone)
        {
            checkCount();
        }

        if (lowerDown)
        {
            model.material = openMat;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -ogY, Time.deltaTime * gateSpeed), transform.position.z);
        }
        else if (!lowerDown)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, ogY, Time.deltaTime * gateSpeed), transform.position.z);
        }
    }

    void checkCount()
    {
        isCounting = true;
        int scrap = 0;
        int listSize = spawnerTrackers.Length;

        bool collected = false;
        SpawnerTracker spawnerTrackr;
        RangedSpawnerTracker rangSpawnrTrackr;

        for(int i = 0; i < listSize; i++)
        {
            spawnerTrackr = spawnerTrackers[i].GetComponent<SpawnerTracker>();
            rangSpawnrTrackr = spawnerTrackers[i].GetComponent<RangedSpawnerTracker>();

            if (spawnerTrackr != null)
            {
                collected = spawnerTrackr.getScrapCollected();
            }
            else if(rangSpawnrTrackr != null)
            {
                collected = rangSpawnrTrackr.getScrapCollected();
            }

            if (collected)
            {
                scrap++;
            }
        }

        if(scrap == listSize)
        {
            spawnerDone = true;

            // audio play for gate open
            audSource.clip = gateOpen;
            if (!audSource.isPlaying) { audSource.Play(); }

            lowerDown = true;
        }

        isCounting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // audio play for gate close
            audSource.clip = gateClose;
            if (!audSource.isPlaying) { audSource.Play(); }

            lowerDown = false;
        }
    }
}
