using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTrigger : MonoBehaviour
{
    IDamage dmg;
    IPhysics phys;
    public Collider playerCollider;
    bool inRange; // player is in range

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.CompareTag("Player"))
        {
            dmg = other.GetComponent<IDamage>();
            phys = other.GetComponent<IPhysics>();
            playerCollider = other;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // player has walked out of explode range, so they should not get damaged
        dmg = null;
        phys = null;
        playerCollider = null;
        inRange = false;
    }

    public IDamage getDmg() { return dmg; }

    public IPhysics getPhys() { return phys; }

    public bool getInRange() { return inRange; }
}
