using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageBuff : MonoBehaviour
{
    // This will buff the gun's damage for a short amount of time
    [Range(1, 5)] [SerializeField] int buffTimer;
    [SerializeField] int buffDamage;
    int OriginalDamage;
    bool used;
    void OnTriggerEnter(Collider other)
    {
        if(!used){
            if(other.CompareTag("Player")){
                OriginalDamage = GameManager.instance.playerScript.shootDamage;
                StartCoroutine(BuffDamage());
            }
        }
    }

    IEnumerator BuffDamage(){
        used = !used;

        GetComponent<MeshRenderer>().enabled = false;
        GameManager.instance.playerScript.shootDamage = buffDamage;

        yield return new WaitForSeconds(buffTimer);

        GameManager.instance.playerScript.shootDamage = OriginalDamage;
        Destroy(gameObject);
    }
}
