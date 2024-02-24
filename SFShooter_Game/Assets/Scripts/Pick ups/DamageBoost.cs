using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoost : MonoBehaviour
{
    [SerializeField] float dmgBoostTime;
    [SerializeField] int dmgBoostAmount;

    bool goUp, isSwitching;
    float ogY, offsetY;

    // Start is called before the first frame update
    void Start()
    {
        goUp = true;
        ogY = transform.position.y;
        offsetY = transform.position.y + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * 50);

        // bob up and down
        if (goUp)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, offsetY, Time.deltaTime * 0.5f), transform.position.z);
        }
        else if (!goUp)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, ogY, Time.deltaTime * 0.5f), transform.position.z);
        }

        // switch between going up and going down
        if (!isSwitching)
        {
            StartCoroutine(floatSwitch());
        }
    }
    IEnumerator floatSwitch()
    {
        isSwitching = true;

        yield return new WaitForSeconds(1);
        goUp = !goUp;

        isSwitching = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Damage Boost Collected");
            this.StartCoroutine(DMGPowerUp());
        }
    }
    IEnumerator DMGPowerUp()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        Destroy(this.gameObject);
        Debug.Log("Object Destroyed");
        int orig = PlayerController.instance.shootDamage;
        Debug.Log("Orig Damage Saved");
        PlayerController.instance.shootDamage += dmgBoostAmount;
        Debug.Log("New DMG Saved");
        yield return new WaitForSeconds(dmgBoostTime);
        PlayerController.instance.shootDamage = orig;
        Debug.Log("IEnum End");
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
