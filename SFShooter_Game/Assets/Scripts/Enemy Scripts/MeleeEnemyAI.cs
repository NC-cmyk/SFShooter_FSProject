using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    [SerializeField] int attackDmg;
    [SerializeField] float attackRange;
    [SerializeField] int attackRate; // to be removed when animations are added

    bool isAttacking;

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

            if (!isAttacking)
                StartCoroutine(attack());
        }

        Debug.DrawRay(transform.position, transform.forward * attackRange);
    }

    IEnumerator attack()
    {
        isAttacking = true;
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
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
