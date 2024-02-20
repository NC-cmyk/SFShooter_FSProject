using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    [Header("MELEE ENEMY DAMAGE IN MELEE HITBOX GAMEOBJECT")]
    [Header("--- Melee Enemy Stats ---")]
    [Range(3, 8)] [SerializeField] int attackRate;
    [Range(3, 5)] [SerializeField] int attackFOV; // field of vision for attacking
    [Range(4, 10)] [SerializeField] int sightDistance; // for rotating because melee stopping distance is too small for the enemy to track the player with
    [Range(15, 25)] [SerializeField] int chargeDistance; // minimum amount of distance for enemy to start charging

    [Header("--- Melee Enemy Components ---")]
    [SerializeField] BoxCollider chargeHitbox;
    [SerializeField] ParticleSystem chargeSmoke;

    float startAccel; // starting acceleration
    float startSpeed;
    float startAngSpeed; // starting angular speed
    bool isAttacking;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startAccel = getAgent().acceleration;
        startSpeed = getAgent().speed;
        startAngSpeed = getAgent().angularSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!gettingDestroyed)
        {
            if (playerInRange && !canSeePlayer())
            {
                StartCoroutine(roam());
            }
            else if (!playerInRange)
            {
                StartCoroutine(roam());
            }
        }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (!isAttacking && canSee)
        {
            //&& !getAnimator().GetBool("isAttacking")
            StopCoroutine(roam());

            // enemy should rotate to face player
            if (getAgent().remainingDistance < sightDistance)
                faceTarget();

            if (angleToPlayer < attackFOV && getAgent().remainingDistance < chargeDistance)
                StartCoroutine(attack());
        }

        return canSee;
    }

    public override void takeDamage(int amount)
    {
        if (!isAttacking)
        {
            getAnimator().SetTrigger("Hit");
        }

        base.takeDamage(amount);

        if (getHP() < 1 && !getAnimator().GetBool("isDead"))
        {
            StartCoroutine(die());
        }
    }

    IEnumerator die()
    {
        getAnimator().ResetTrigger("Hit");
        getAnimator().SetBool("isDead", true);

        // prevent enemy from moving
        gettingDestroyed = true;
        stopAttacking();
        getAgent().angularSpeed = 0;
        getAgent().speed = 0;
        StopCoroutine(roam());

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    IEnumerator attack()
    {
        isAttacking = true;
        chargeSmoke.Play();
        getAnimator().SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.3f);

        // when attacking, make hitbox active
        chargeHitbox.enabled = !chargeHitbox.enabled;

        getAgent().stoppingDistance = 0;
        getAgent().acceleration = 20;
        getAgent().speed = 15;
        getAgent().angularSpeed = 0;

        //Vector3 destination = playerDir + transform.position;

        yield return new WaitForSeconds(1);

        chargeHitbox.GetComponent<MeleeHitbox>().hit = false;
        chargeHitbox.enabled = !chargeHitbox.enabled;

        getAgent().stoppingDistance = stoppingDistOrig;
        getAgent().acceleration = startAccel;
        getAgent().speed = startSpeed;
        getAgent().angularSpeed = startAngSpeed;
        getAnimator().SetBool("isAttacking", false);

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    void stopAttacking()
    {
        StopCoroutine(attack());
        chargeHitbox.enabled = false;
    }
}
