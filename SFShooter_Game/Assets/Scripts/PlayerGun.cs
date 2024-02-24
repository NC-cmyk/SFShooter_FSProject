using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    Vector3 ogRotation;
    Vector3 recoil;
    float shootRate;
    bool isShooting;
    float recoverSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ogRotation = transform.localEulerAngles;
        recoil = new Vector3(0, 0, 15);
        shootRate = GameManager.instance.playerScript.shootRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Shoot") && !isShooting && !GameManager.instance.isPaused)
        {
            // rotate up once
            StartCoroutine(shoot());
        }

        if (!GameManager.instance.isPaused)
        {
            float angle = Mathf.LerpAngle(transform.localEulerAngles.z, ogRotation.z, Time.deltaTime * 5);
            transform.localEulerAngles = new Vector3(ogRotation.x, ogRotation.y, angle);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        transform.localEulerAngles += recoil;
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
