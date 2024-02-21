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

    private void Update()
    {
        // bullet should try and travel towards player after being spawned
        Vector3 targetPosition = GameManager.instance.player.transform.position - transform.position;
        targetPosition.y = 0;

        Vector3 currDirection = transform.forward;

        // slowly rotate towards player with max turn speed
        Vector3 direction = Vector3.RotateTowards(currDirection, targetPosition, 90 * Mathf.Deg2Rad * Time.deltaTime, 1);

        // change the rotation
        transform.rotation = Quaternion.LookRotation(direction);
        getRB().velocity = transform.forward * getSpeed();

        // destroy immediately if boss dies
        if (!GameManager.instance.bossActive)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
