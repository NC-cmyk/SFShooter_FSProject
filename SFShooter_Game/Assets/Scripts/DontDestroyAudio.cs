using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    private void Awake()
    {
        // returning to the main menu from the game can cause dupes because of the DontDestroyOnLoad()
        GameObject[] dupeBGMusic = GameObject.FindGameObjectsWithTag("Background Music");
        foreach (GameObject bgMusic in dupeBGMusic)
        {
            if (bgMusic != gameObject)
            {
                Destroy(bgMusic);
            }
        }

        DontDestroyOnLoad(transform.gameObject);
    }

}
