using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        // add to the scrap needed 
    }

    // Update is called once per frame
    void Update()
    {
        // make collectable when all enemies are dead

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            Debug.Log("Scrap collected");
            ScrapTracker.instance.CollectScrap();
            Destroy(gameObject);
        }
    }

}
