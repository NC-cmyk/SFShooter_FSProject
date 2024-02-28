using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVerticalMin;
    [SerializeField] int lockVerticalMax;
    
    float xRotation;
    bool invertY;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (PlayerPrefs.HasKey("masterInvert"))
        {
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
