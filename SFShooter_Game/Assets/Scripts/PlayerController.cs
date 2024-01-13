using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    

    [SerializeField] float playerSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    Vector3 playerVelocity;
    Vector3 move;
    bool groundedPlayer;
    int jumpCount;
    bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        if(Input.GetButton("Shoot") && !isShooting){
            StartCoroutine(shoot());
        }
        
        movement();
    }

    public void movement(){
        groundedPlayer = controller.isGrounded;

        if(groundedPlayer){
            jumpCount = 0;
        }

        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax){
            playerVelocity.y = jumpHeight;
            jumpCount++;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator shoot(){
        isShooting = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(.5f, .5f)), out hit, shootDistance)){
            IDamage damage = hit.collider.GetComponent<IDamage>();

            if(damage != null){
                damage.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    
}
