using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    [Header("--- Ranged Enemy Components ---")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    [Header("RANGED ENEMY DAMAGE IN BULLET PREFAB")]
    [Header("--- Ranged Enemy Stats ---")]
    [Range(0.1f, 1)] [SerializeField] float shootRate;
    [Range(10, 45)] [SerializeField] int shootFOV; // fov for shooting/attacking

    bool isShooting;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); // just animations

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (canSee)
        {
            StopCoroutine(roam());

            // enemy should rotate to face player even if player is within stopping distance
            if (getAgent().remainingDistance < getAgent().stoppingDistance)
                faceTarget();

            if (angleToPlayer < shootFOV && !isShooting)
            {
                StartCoroutine(shoot());
            }
        }

        return canSee;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
