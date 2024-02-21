using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : EnemyAI
{
    [Header("--- Boss Components ---")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform[] summonPositions;
    [SerializeField] Transform[] bulletPositions;
    [SerializeField] GameObject[] enemyList;
    [SerializeField] GameObject shield;

    [Header("--- Boss Stats ---")]
    [Range(5, 10)] [SerializeField] int summonCooldown;
    [Range(10, 15)] [SerializeField] int shieldTimer;
    [Range(1, 10)] [SerializeField] int attackRate;
    [Range(10, 45)] [SerializeField] int attackFOV;

    [Header("--- Audio Clips ---")]
    [SerializeField] AudioClip shootSound;
    [Range(0, 1)] [SerializeField] float soundVolume;

    float startSpeed;
    int minionCount;
    int maxHP;

    bool isSummoning;
    bool isAttacking;
    bool shieldCoolingDown;
    bool summoned; // boss has resummoned enemies and restored shield

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startSpeed = getAgent().speed;
        maxHP = getHP();
        shield.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!gettingDestroyed)
        {
            base.Update();

            if (playerInRange)
            {
                GameManager.instance.bossActive = true;
                canSeePlayer();
            }
        }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (canSee)
        {
            if(!isAttacking)
                faceTarget();

            if(!summoned && !isSummoning && !isAttacking && minionCount == 0)
            {
                StartCoroutine(summon());
            }
            else if (summoned && minionCount == 0 && !shieldCoolingDown)
            {
                StartCoroutine(shieldCooldown());
            }
            
            if(angleToPlayer < attackFOV && !isAttacking && !getAnimator().GetBool("isSummoning"))
            {
                StartCoroutine(attack());
            }
        }

        return canSee;
    }

    IEnumerator summon()
    {
        isSummoning = true;
        getAnimator().SetBool("isSummoning", isSummoning);
        getAgent().speed = 0;
        yield return new WaitForSeconds(1);

        // restore shield
        shield.SetActive(true);

        // summon enemies
        int enemyNdx = Random.Range(0, 2);

        GameObject enemy = enemyList[enemyNdx];

        for(int i = 0; i < summonPositions.Length; i++)
        {
            Instantiate(enemy, summonPositions[i].transform.position, summonPositions[i].transform.rotation);
            minionCount++;
        }

        // prevents boss from summoning right away if minions are dead
        summoned = true;

        getAnimator().SetBool("isSummoning", false);
        getAgent().speed = startSpeed;

        yield return new WaitForSeconds(summonCooldown);
        isSummoning = false;
    }

    IEnumerator attack()
    {
        isAttacking = true;
        getAnimator().SetBool("isAttacking", isAttacking);
        getAgent().speed = 0;
        yield return new WaitForSeconds(1);

        for (int i = 0; i < bulletPositions.Length; i++)
        {
            getAudSource().PlayOneShot(shootSound, soundVolume);
            Instantiate(bullet, bulletPositions[i].transform.position, bulletPositions[i].transform.rotation);
            yield return new WaitForSeconds(0.2f);
        }

        getAnimator().SetBool("isAttacking", false);
        getAgent().speed = startSpeed;

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public override void takeDamage(int amount)
    {
        if (!getAnimator().GetBool("isDead"))
        {
            if(!getAnimator().GetBool("isAttacking") && !getAnimator().GetBool("isSummoning"))
            {
                getAnimator().SetTrigger("Hit");
            }

            base.takeDamage(amount);

            GameManager.instance.bossHPBar.fillAmount = (float)getHP() / maxHP;

            if (getHP() <= 0)
            {
                GameManager.instance.bossActive = false;
                gettingDestroyed = true;

                StartCoroutine(die());
            }
        }
    }

    public void updateMinionCount(int amount)
    {
        minionCount += amount;
    }

    IEnumerator die()
    {
        getAnimator().SetBool("isDead", true);

        // prevent enemy from moving
        getAgent().isStopped = true;

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    IEnumerator shieldCooldown()
    {
        shieldCoolingDown = true;
        shield.SetActive(false);

        yield return new WaitForSeconds(shieldTimer);

        // allows boss to resummon enemies and put shield back up
        summoned = false;

        shieldCoolingDown = false;
    }
}
