using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("--- Component ---")]
    [SerializeField] Rigidbody rb;

    [Header("--- Stats ---")]
    [SerializeField] int dmgAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        // bullet go towards player
        rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && other.CompareTag("Player"))
            dmg.takeDamage(dmgAmount);

        Destroy(gameObject);
    }
}
