using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("--- Components ---")]
    public Renderer model;  // change back to SerializeField when getters/setters are added
    [SerializeField] NavMeshAgent agent;

    [Header("--- General Stats ---")]
    [SerializeField] int HP;

    protected bool playerInRange;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.instance.GameGoalUpdate(1);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            GameManager.instance.GameGoalUpdate(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        // may be removed once animations are implemented
        Color ogColor = model.material.color;
        
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = ogColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
