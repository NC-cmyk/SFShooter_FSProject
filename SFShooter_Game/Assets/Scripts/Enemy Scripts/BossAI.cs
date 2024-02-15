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

    [Header("--- Boss Stats ---")]
    [Range(5, 10)] [SerializeField] int summonCooldown;
    [Range(3, 10)] [SerializeField] int attackRate;
    [Range(10, 45)] [SerializeField] int attackFOV;

    [Header("--- Audio Clips ---")]
    [SerializeField] AudioClip shootSound;
    [Range(0, 1)] [SerializeField] float soundVolume;

    float startSpeed;
    int minionCount;
    int maxHP;

    bool isSummoning;
    bool isAttacking;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startSpeed = getAgent().speed;
        maxHP = getHP();
        GameManager.instance.bossActive = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (playerInRange && !canSeePlayer()) { }
    }

    protected override bool canSeePlayer()
    {
        bool canSee = base.canSeePlayer();

        if (canSee)
        {
            if(!isAttacking)
                faceTarget();

            if(angleToPlayer < attackFOV && !isSummoning && !isAttacking && minionCount == 0)
            {
                StartCoroutine(summon());
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

        int enemyNdx = Random.Range(0, 2);

        GameObject enemy = enemyList[enemyNdx];
        enemy.tag = "Minion";

        for(int i = 0; i < 2; i++)
        {
            Instantiate(enemy, summonPositions[i].transform.position, summonPositions[i].transform.rotation);
            minionCount++;
        }

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
        getAudSource().PlayOneShot(getHurtSound(), getHurtVolume());
        setHP(getHP() - amount);

        GameManager.instance.bossHPBar.fillAmount = (float)getHP() / maxHP;

        if(getHP() <= 0)
        {
            GameManager.instance.bossActive = false;
            Destroy(gameObject);
        }
    }

    public void updateMinionCount(int amount)
    {
        minionCount += amount;
    }
}
