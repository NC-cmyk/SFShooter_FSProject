using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : Bullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        // basically does the same thing as the parent but the bullet travels from the forward it instantiated from
        getRB().velocity = transform.forward * getSpeed();
        Destroy(gameObject, getDestroyTime());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
