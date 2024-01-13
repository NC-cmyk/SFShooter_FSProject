using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;

    [SerializeField] GameObject player; // will be removed once game manager is implemented

    // Start is called before the first frame update
    void Start()
    {
        // add code to let game manager know to add to enemy count
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());

        if(HP <= 0)
        {
            // update game manager goal
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        // may be removed once animations are implemented
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
