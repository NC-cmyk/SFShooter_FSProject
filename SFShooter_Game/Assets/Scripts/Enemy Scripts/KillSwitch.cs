using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSwitch : MonoBehaviour, IDamage
{
    [Header("--- Kill Switch Components ---")]
    [SerializeField] Renderer model;
    [SerializeField] AudioSource audSource;

    bool hit;
    bool realSwitch;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        model.material.color = Color.green;
    }

    public void takeDamage(int damage)
    {
        // doesnt actually take damage, this is just to register that it was hit
        if (!hit)
        {
            model.material.color = Color.red;
            hit = true;
        }

        if (realSwitch)
        {
            audSource.Play();
        }
    }

    public bool getHit()
    {
        return hit;
    }

    public void setRealSwitch(bool isReal)
    {
        realSwitch = isReal;
    }
}
