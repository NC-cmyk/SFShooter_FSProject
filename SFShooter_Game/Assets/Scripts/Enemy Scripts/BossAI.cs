using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : EnemyAI
{
    [Header("--- Boss Components ---")]
    [SerializeField] Transform[] summonPositions;
    [SerializeField] Transform[] bulletPositions;
    [SerializeField] GameObject[] enemyList;
    [SerializeField] GameObject bullet;

    [Header("--- Boss Stats ---")]
    [Range(5, 10)] [SerializeField] int summonCooldown;
    [Range(3, 10)] [SerializeField] int attackRate;
    [Range(10, 45)] [SerializeField] int attackFOV;

    float startSpeed;
    int minionCount;
    int maxHP;

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
            faceTarget();

            if(angleToPlayer < attackFOV && !getAnimator().GetBool("isSummoning") && minionCount == 0)
            {
                StartCoroutine(summon());
            }
        }

        return canSee;
    }

    IEnumerator summon()
    {
        getAgent().speed = 0;
        getAnimator().SetBool("isSummoning", true);

        int enemyNdx = Random.Range(0, 2);

        GameObject enemy = enemyList[enemyNdx];
        enemy.tag = "Minion";

        for(int i = 0; i < 2; i++)
        {
            Instantiate(enemy, summonPositions[i].transform.position, summonPositions[i].transform.rotation);
            minionCount++;
            yield return new WaitForSeconds(1);
        }

        getAnimator().SetBool("isSummoning", false);
        getAgent().speed = startSpeed;
        yield return new WaitForSeconds(summonCooldown);
    }

    IEnumerator attack()
    {
        yield return null;
    }

    public override void takeDamage(int amount)
    {
        getAudSource().PlayOneShot(getHurtSound(), getHurtVolume());
        setHP(getHP() - amount);

        GameManager.instance.bossHPBar.fillAmount = (float)getHP() / maxHP;
    }

    public void updateMinionCount(int amount)
    {
        minionCount += amount;
    }
}
