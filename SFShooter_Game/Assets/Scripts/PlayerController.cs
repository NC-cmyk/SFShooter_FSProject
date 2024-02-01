using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    

    [SerializeField] int HP;
    [SerializeField] int shieldHP;
    [SerializeField] int shieldTimer;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpTimer;
    [SerializeField] float gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    Vector3 playerVelocity;
    Vector3 move;
    int shieldHPmax;
    int HPmax;
    bool groundedPlayer;
    int jumpMax = 2;
    int jumpCount;
    bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        shieldHPmax = shieldHP;
        HPmax = HP;
        respawn();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        if(Input.GetButton("Shoot") && !isShooting && !GameManager.instance.isPaused)
        {
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

            if(jumpCount == 2){
                StartCoroutine(jumpResetTimer());
            }
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

    IEnumerator jumpResetTimer(){
        jumpMax = 1;
        yield return new WaitForSeconds(jumpTimer);
        jumpMax = 2;

    }

    IEnumerator shieldCharger(){

        yield return new WaitForSeconds(shieldTimer);
        shieldHP = shieldHPmax;
    }

    public void takeDamage(int amount){
        if(shieldHP <= 0 || amount < 0){
            HP -= amount;
        }
        else{
            shieldHP -= amount;
            StopCoroutine(shieldCharger());
            StartCoroutine(shieldCharger());
        }

        if(HP <= 0){
            // This is where the player dies, Game over screen
            GameManager.instance.youLose();
        }

        updatePlayerUI();
    }

    public void respawn(){
        HP = HPmax;
        shieldHP = shieldHPmax;

        // update UI
        updatePlayerUI();
        controller.enabled = false;
        // use the spawn position to move the player
        transform.position = GameManager.instance.playerSpawnPosition.transform.position;
        controller.enabled = true;
    }

    public void updatePlayerUI(){
        // update hp bar from GameManager
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPmax;
    }

    IEnumerator flashDamage(){
        // show damage flash
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Scrap"))
    //     {
    //         ScrapTracker.instance.CollectScrap();

    //         // might want to disable or destroy the collected scrap GameObject
    //         other.gameObject.SetActive(false);
    //     }
    // }
}

