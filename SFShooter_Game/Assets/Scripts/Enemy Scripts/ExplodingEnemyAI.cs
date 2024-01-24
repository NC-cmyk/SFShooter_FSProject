using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ExplodingEnemyAI : EnemyAI
{
    [Header("--- Exploding Enemy Components ---")]
    [SerializeField] Collider explodeTrigger;
    [SerializeField] GameObject explosion;

    [Header("--- Exploding Enemy Stats ---")]
    [Range(3, 10)] [SerializeField] int explodeDmg;
    [Range(3, 5)] [SerializeField] int explodeTimer;

    bool isExploding;

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

            if(explodeTrigger.GetComponent<ExplodeTrigger>().inRange && !isExploding)
            {
                StartCoroutine(explode());
            }
        }
    }

    IEnumerator explode()
    {
        isExploding = true;
        StartCoroutine(flashWarning());
        yield return new WaitForSeconds(explodeTimer);

        if(explodeTrigger.GetComponent<ExplodeTrigger>().dmg != null)
            explodeTrigger.GetComponent<ExplodeTrigger>().dmg.takeDamage(explodeDmg);

        explosion.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<IDamage>().takeDamage(explodeDmg);
    }

    IEnumerator flashWarning()
    {
        Color ogColor = model.material.color;

        while (isExploding)
        {
            model.material.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            model.material.color = ogColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
