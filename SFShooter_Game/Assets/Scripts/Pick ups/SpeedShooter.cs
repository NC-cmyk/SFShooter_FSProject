using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedShooter : MonoBehaviour
{
    // This will buff the gun's shoot rate for a short amount of time
    [Range(1, 5)] [SerializeField] int buffTimer;
    [Range(2, 5)] [SerializeField] float BuffRate;

    float OriginalRate;
    bool used;


    void OnTriggerEnter(Collider other)
    {
        if(!used){
            if(other.CompareTag("Player")){
                OriginalRate = GameManager.instance.playerScript.shootRate;
                StartCoroutine(BuffSpeed());
            }
        }
    }
    IEnumerator BuffSpeed(){
        used = !used;

        GetComponent<MeshRenderer>().enabled = false;
        GameManager.instance.playerScript.shootRate = OriginalRate / BuffRate;
        
        yield return new WaitForSeconds(buffTimer);

        GameManager.instance.playerScript.shootRate = OriginalRate;
        Destroy(gameObject);
    }
    
}
