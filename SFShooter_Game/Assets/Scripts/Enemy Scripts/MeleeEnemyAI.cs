using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    [Header("--- Melee Enemy Stats ---")]
    [Range(1, 3)] [SerializeField] int attackDmg;
    [Range(1, 3)] [SerializeField] float attackRange;
    [Range(1, 2)] [SerializeField] int attackRate; // to be removed when animations are added
    [Range(3, 5)] [SerializeField] int attackFOV; // field of vision for attacking
    [Range(4, 10)] [SerializeField] int sightDistance; // for rotating because melee stopping distance is too small for the enemy to track the player with

    bool isAttacking;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (playerInRange)
        {
            if (canSeePlayer()) { }
        }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (canSee)
        {
            // enemy should rotate to face player
            if (getAgent().remainingDistance < sightDistance)
                faceTarget();

            if (angleToPlayer < attackFOV && !isAttacking)
                StartCoroutine(attack());
        }

        Debug.DrawRay(getHeadPos().position, transform.forward * attackRange);

        return canSee;
    }

    IEnumerator attack()
    {
        isAttacking = true;
        RaycastHit hit;

        if(Physics.Raycast(getHeadPos().position, transform.forward, out hit, attackRange))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null && hit.collider.CompareTag("Player"))
            {
                dmg.takeDamage(attackDmg);
            }
        }
        yield return new WaitForSeconds(attackRate);

        isAttacking = false;
    }
}
