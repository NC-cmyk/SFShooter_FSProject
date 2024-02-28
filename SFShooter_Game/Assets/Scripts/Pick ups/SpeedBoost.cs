using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] float speedBoostTime;
    [Range(1,2)] [SerializeField] float speedBoostMultiplier;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerController.instance.isPowerUpCoroutineRunning)
        {
            StartCoroutine(PlayerController.instance.SpeedPowerUp(speedBoostTime, speedBoostMultiplier, this.gameObject));
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
