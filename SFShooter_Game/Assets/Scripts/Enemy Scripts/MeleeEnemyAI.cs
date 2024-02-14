using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    [Header("MELEE ENEMY DAMAGE IN MELEE HITBOX GAMEOBJECT")]
    [Header("--- Melee Enemy Stats ---")]
    [Range(1, 3)] [SerializeField] float attackRange;
    [Range(3, 8)] [SerializeField] int attackRate;
    [Range(3, 5)] [SerializeField] int attackFOV; // field of vision for attacking
    [Range(4, 10)] [SerializeField] int sightDistance; // for rotating because melee stopping distance is too small for the enemy to track the player with

    [Header("--- Melee Enemy Components ---")]
    [SerializeField] BoxCollider chargeHitbox;

    float startAccel; // starting acceleration
    float startSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startAccel = getAgent().acceleration;
        startSpeed = getAgent().speed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (playerInRange && !canSeePlayer())
        {
            stopAttacking();
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            stopAttacking();
            StartCoroutine(roam());
        }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (canSee && !getAnimator().GetBool("isAttacking"))
        {
            //&& !getAnimator().GetBool("isAttacking")
            StopCoroutine(roam());

            // enemy should rotate to face player
            if (getAgent().remainingDistance < sightDistance)
                faceTarget();

            if (angleToPlayer < attackFOV)
                StartCoroutine(attack());
        }

        return canSee;
    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(1.0f);
        getAnimator().SetBool("isAttacking", true);

        // when attacking, make hitbox active
        chargeHitbox.enabled = !chargeHitbox.enabled;

        getAgent().acceleration = 20;
        getAgent().speed = 15;

        yield return new WaitForSeconds(attackRate);

        chargeHitbox.GetComponent<MeleeHitbox>().hit = false;
        chargeHitbox.enabled = !chargeHitbox.enabled;
        getAgent().acceleration = startAccel;
        getAgent().speed = startSpeed;
        getAnimator().SetBool("isAttacking", false);
    }

    void stopAttacking()
    {
        StopCoroutine(attack());
        chargeHitbox.enabled = false;
        getAgent().acceleration = startAccel;
        getAgent().speed = startSpeed;
        getAnimator().SetBool("isAttacking", false);
    }
}
