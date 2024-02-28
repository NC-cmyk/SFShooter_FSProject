using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] int healthPackAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerController.instance.audSource.PlayOneShot(PlayerController.instance.powerupSound);
            PlayerController.instance.HP += healthPackAmount;
        }
    }

}
