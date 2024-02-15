using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("--- Component ---")]
    [SerializeField] Rigidbody rb;

    [Header("--- Stats ---")]
    [Range(1, 3)] [SerializeField] int dmgAmount;
    [Range(10, 80)] [SerializeField] int speed;
    [Range(3, 5)] [SerializeField] int destroyTime;
    [Range(1, 20)] [SerializeField] int knockbackDist;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // bullet go towards player
        rb.velocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, destroyTime);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();
        IPhysics phys = other.GetComponent<IPhysics>();

        if (dmg != null && other.CompareTag("Player"))
            dmg.takeDamage(dmgAmount);

        if (phys != null && other.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0;

            phys.takePhysics(direction.normalized * knockbackDist);
        }

        Destroy(gameObject);
    }

    protected Rigidbody getRB() { return rb; }
    protected int getSpeed() { return speed; }
    protected int getDestroyTime() { return destroyTime; }
}
