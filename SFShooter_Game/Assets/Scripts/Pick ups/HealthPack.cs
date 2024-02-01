using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    [SerializeField] int HealAmount;
    int healing;
    // Start is called before the first frame update
    void Start()
    {
        healing = HealAmount * -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && other.CompareTag("Player")){
            dmg.takeDamage(healing);
            Destroy(gameObject);
        }
    }
}
