using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    bool collectable;
    // Start is called before the first frame update
    void Start()
    {
        collectable = false;
        // add to the scrap needed 
    }

    // Update is called once per frame
    void Update()
    {
        // make collectable when all enemies are dead
    }

    void OnTriggerEnter(Collider other)
    {
        if(collectable){
            if(other.CompareTag("Player")){
                ScrapTracker.instance.CollectScrap();
            }
        }
        else{
            // display reason you can't collect
            
        }

    }
}
