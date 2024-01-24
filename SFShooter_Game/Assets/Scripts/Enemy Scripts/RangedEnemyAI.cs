using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    [Header("--- Ranged Enemy Components ---")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;

    [Header("--- Ranged Enemy Stats ---")]
    [Range(0.1f, 1)] [SerializeField] float shootRate;

    bool isShooting;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (playerInRange)
        {
            base.Update();

            if (!isShooting)
                StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
