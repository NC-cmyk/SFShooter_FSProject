using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoost : MonoBehaviour
{
    [SerializeField] float dmgBoostTime;
    [SerializeField] int dmgBoostAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerController.instance.isPowerUpCoroutineRunning)
        {
            Debug.Log("Damage Boost Collected");
            StartCoroutine(PlayerController.instance.Damage(dmgBoostTime, dmgBoostAmount, this.gameObject));
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            
        }
    }
}
