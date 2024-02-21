using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [Header("--- Container Components ---")]
    [SerializeField] GameObject closedEgg;
    [SerializeField] GameObject openEgg;
    [SerializeField] ParticleSystem eggBurst;

    public void openContainer()
    {
        closedEgg.SetActive(false);
        openEgg.SetActive(true);
        if (!eggBurst.isPlaying)
        {
            eggBurst.Play();
        }
    }
}
