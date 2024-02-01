using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource audSource;
    
    [Header("----- Player Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int shieldHP;
    [SerializeField] int shieldTimer;

    [Header("----- Movement -----")]
    [SerializeField] float playerSpeed;
    [Range(1, 2)] [SerializeField] float sprintModifier;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpTimer;
    [SerializeField] float gravity;

    [Header("----- Gun Stats-----")]
    [SerializeField] public int shootDamage;
    [SerializeField] public float shootRate;
    [SerializeField] int shootDistance;

    [Header("----- Audio Clips -----")]
    [SerializeField] AudioClip playerHurtSound;
    [Range(0, 1)][SerializeField] float hurtSoundVol;
    [SerializeField] AudioClip playerDeathSound;
    [Range(0, 1)][SerializeField] float deathSoundVol;
    [SerializeField] AudioClip playerShootSound;
    [Range(0, 1)][SerializeField] float shootSoundVol;
    [SerializeField] AudioClip playerWinSound;
    [Range(0, 1)][SerializeField] float winSoundVol;
    [SerializeField] AudioClip[] footsteps;
    [Range(0, 1)][SerializeField] float stepSoundVol;

    Vector3 playerVelocity;
    Vector3 move;
    int shieldHPmax;
    int HPmax;
    bool groundedPlayer;
    int jumpMax = 2;
    int jumpCount;
    bool isShooting;
    float sprint;
    bool playingSteps;
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
            audSource.PlayOneShot(playerShootSound, shootSoundVol);
            StartCoroutine(shoot());
        }
        
        movement();
    }

    public void movement(){
        groundedPlayer = controller.isGrounded;

        if(groundedPlayer){
            jumpCount = 0;
        }
        if(!playingSteps && groundedPlayer && move.normalized.magnitude > 0.3f)
            StartCoroutine(playSteps());
        if(Input.GetButton("Sprint")){
            sprint = sprintModifier;
        }
        else{
            sprint = 1;
        }

        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * sprint * Time.deltaTime);

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

    IEnumerator playSteps()
    {
        playingSteps = true;
        audSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
        yield return new WaitForSeconds(0.5f);
        playingSteps = false;
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
        audSource.PlayOneShot(playerHurtSound, hurtSoundVol);
        if (shieldHP <= 0 || amount < 0){
            HP -= amount;
        }
        else{
            shieldHP -= amount;
            StopCoroutine(shieldCharger());
            StartCoroutine(shieldCharger());
        }

        if(HP <= 0){
            audSource.PlayOneShot(playerDeathSound, deathSoundVol);
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
        GameManager.instance.shieldHPBar.fillAmount = (float)shieldHP / shieldHPmax;
    }

    IEnumerator flashDamage(){
        // show damage flash
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }
}

