using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    [Header("MELEE ENEMY DAMAGE IN MELEE HITBOX GAMEOBJECT")]
    [Header("--- Melee Enemy Stats ---")]
    [Range(3, 8)] [SerializeField] int attackRate;
    [Range(5, 10)] [SerializeField] int attackFOV; // field of vision for attacking
    [Range(4, 10)] [SerializeField] int sightDistance; // for rotating because melee stopping distance is too small for the enemy to track the player with
    [Range(15, 25)] [SerializeField] int chargeDistance; // minimum amount of distance for enemy to start charging

    [Header("--- Melee Enemy Components ---")]
    [SerializeField] BoxCollider chargeHitbox;
    [SerializeField] ParticleSystem chargeSmoke;
    [SerializeField] GameObject[] powerups;

    [Header("--- Melee Enemy Audio ---")]
    [SerializeField] AudioClip chargeWarningSFX;

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
            // roam does not start right away, so audio should stop if enemy is stopped
            if (getAgent().remainingDistance < 0.05f)
            {
                getAudSource().Stop();
            }

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
        if (!getAnimator().GetBool("isDead"))
        {
            if (!isAttacking)
            {
                getAnimator().SetTrigger("Hit");
            }

            base.takeDamage(amount);

            if (getHP() < 1)
            {
                StartCoroutine(die());
            }
        }
    }

    IEnumerator die()
    {
        // prevent enemy from moving
        // does allow sliding for if they die while charging though
        gettingDestroyed = true;
        stopAttacking();
        getAgent().angularSpeed = 0;
        getAgent().speed = 0;
        StopCoroutine(roam());

        getAnimator().ResetTrigger("Hit");
        getAnimator().SetBool("isDead", true);

        yield return new WaitForSeconds(2);

        int itemDrop = Random.Range(0, 49);

        if(itemDrop < 15)
        {
        int chosenDrop = Random.Range(0, 100);
            if (chosenDrop < 100 && chosenDrop > 67)
            {
                Instantiate(powerups[0], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), Quaternion.Euler(270, 0, 0));
            }
            else if (chosenDrop < 67 && chosenDrop > 34)
            {
                Instantiate(powerups[1], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), Quaternion.Euler(270, 0, 0));
            }
            else if(chosenDrop < 34)
            {
                Instantiate(powerups[2], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), Quaternion.Euler(270, 0, 0));
            }
        }

        Destroy(gameObject);
    }

    IEnumerator attack()
    {
        isAttacking = true;
        getAnimator().SetBool("isAttacking", true);

        // small time period to get the charging animation started
        yield return new WaitForSeconds(0.3f);

        // stop walking sfx if its playing
        if (getAudSource().isPlaying)
        {
            getAudSource().Stop();
        }

        // charge warning sfx
        getAudSource().PlayOneShot(chargeWarningSFX);

        // charge smoke vfx
        chargeSmoke.Play();

        // when attacking, make hitbox active
        chargeHitbox.enabled = !chargeHitbox.enabled;

        // prevents enemy from stopping prematurely
        getAgent().stoppingDistance = 0;

        // makes enemy faster
        getAgent().acceleration = 20;
        getAgent().speed = 15;

        // enemy should not be able to turn while charging
        getAgent().angularSpeed = 0;

        yield return new WaitForSeconds(1);

        // restore hitbox to original state
        chargeHitbox.GetComponent<MeleeHitbox>().hit = false;
        chargeHitbox.enabled = !chargeHitbox.enabled;

        // restore enemy's agent stats
        getAgent().stoppingDistance = stoppingDistOrig;
        getAgent().acceleration = startAccel;
        getAgent().speed = startSpeed;
        getAgent().angularSpeed = startAngSpeed;

        // if enemy is still moving, walking should resume
        if (!getAgent().isStopped)
        {
            getAudSource().PlayDelayed(2);
        }

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
