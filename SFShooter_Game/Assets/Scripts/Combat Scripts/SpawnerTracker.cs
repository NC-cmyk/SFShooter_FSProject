using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTracker : MonoBehaviour
{
    [Header("--- Spawner Tracker Components ---")]
    [SerializeField] Transform trackerTransform;
    [SerializeField] GameObject container; // container that holds the scrap
    [SerializeField] GameObject scrap;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip scrapCollectSound;
    [Range(0f, 1f)] [SerializeField] float collectSoundVol;

    int enemyCount;
    bool countAssigned;
    bool scrapCollected;

    //audioSource.PlayOneShot(scrapCollectSound, collectSoundVol); --- This is the line of code to play the collection sound. I have no idea where to put it to make it work though

    // Start is called before the first frame update
    protected virtual void Start()
    {
        countAssigned = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (countAssigned)
        {
            enemyCount = trackerTransform.childCount;
            if(enemyCount <= 0)
            {
                container.SetActive(false);
            }

            if(scrap == null)
            {
                scrapCollected = true;
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

    protected Transform getTrackerTransform()
    {
        return trackerTransform;
    }

    public bool getScrapCollected()
    {
        return scrapCollected;
    }
}
