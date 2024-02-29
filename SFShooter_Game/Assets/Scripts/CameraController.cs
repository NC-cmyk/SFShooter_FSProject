using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int lockVerticalMin;
    [SerializeField] int lockVerticalMax;

    int sensitivity;
    float xRotation;
    bool invertY;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // check if masterInvert key exists
        if (PlayerPrefs.HasKey("MasterSens"))
        {
            // set camera sensitivity to MasterSens if it does
            sensitivity = PlayerPrefs.GetInt("MasterSens");
        }
        else
        {
            // if it doesnt, set sensitivity to 100
            sensitivity = 100;
            // NOTE: not likely that it doesnt exist but just in case
        }

        // check if masterInvert key exists
        if (PlayerPrefs.HasKey("masterInvert"))
        {
            // if its 1, then set invertY to true, otherwise it defaults to false
            if(PlayerPrefs.GetInt("masterInvert") == 1)
            {
                invertY = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        
        if (invertY)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }
        
        xRotation = Mathf.Clamp(xRotation, lockVerticalMin, lockVerticalMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
