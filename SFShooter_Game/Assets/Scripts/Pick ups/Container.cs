using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [Header("--- Container Components ---")]
    [SerializeField] GameObject closedEgg;
    [SerializeField] GameObject openEgg;
    [SerializeField] ParticleSystem eggBurst;

    [Header("--- Container Audio ---")]
    [SerializeField] AudioSource audioSource;

    bool audioPlayed;

    public void openContainer()
    {
        closedEgg.SetActive(false);

        // particle system
        if (!eggBurst.isPlaying)
        {
            eggBurst.Play();
        }

        // container opening sfx
        if (!audioPlayed)
        {
            audioPlayed = true;
            audioSource.Play();
        }

        openEgg.SetActive(true);
    }
}
