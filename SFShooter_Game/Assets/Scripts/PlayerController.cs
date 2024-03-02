using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IPhysics
{
    public static PlayerController instance;
    [SerializeField] CharacterController controller;
    public Camera cam;
    [SerializeField] public AudioSource audSource;
    
    [Header("----- Player Stats -----")]
    [SerializeField] public int HP;
    [SerializeField] float shieldHP;
    [SerializeField] int shieldTimer;

    [Header("----- Movement -----")]
    [SerializeField] public float playerSpeed;
    [Range(1, 2)] [SerializeField] float sprintModifier;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;
    [SerializeField] int knockbackResolve;

    [Header("----- Gun Stats-----")]
    [SerializeField] public int shootDamage;
    [Range(0, 1)] [SerializeField] public float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] ParticleSystem shootEffect;

    [Header("----- Audio Clips -----")]
    [SerializeField] AudioClip playerHurtSound;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] AudioClip playerShootSound;
    [SerializeField] public AudioClip playerWinSound;
    [SerializeField] AudioClip playerJumpSound;
    [SerializeField] AudioClip scrapCollectedSound;
    [SerializeField] public AudioClip powerupSound;
    [SerializeField] AudioClip powerdownSound;

    Vector3 playerVelocity;
    Vector3 move;
    Vector3 knockback;
    float shieldHPmax;
    int HPmax;
    bool groundedPlayer;
    bool isShooting;
    bool shieldRecharging;
    float sprint;
    public bool isPowerUpCoroutineRunning;

    void Awake()
    {
        instance = this;
    }
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
        if(Input.GetButton("Shoot") && !isShooting && !GameManager.instance.isPaused)
        {
            StartCoroutine(shoot());
        }

        if (shieldRecharging)
        {
            shieldHP = Mathf.Lerp(shieldHP, shieldHPmax, Time.deltaTime * 5);
            updatePlayerUI();
        }
        
        movement();
    }

    public void movement(){
        knockback = Vector3.Lerp(knockback, Vector3.zero, Time.deltaTime * knockbackResolve);

        groundedPlayer = controller.isGrounded;

        if (Input.GetButton("Sprint")){
            sprint = sprintModifier;
        }
        else{
            sprint = 1;
        }

        move = (Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward).normalized;

        controller.Move(move * playerSpeed * sprint * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && groundedPlayer){
            playerVelocity.y = jumpHeight;
            audSource.PlayOneShot(playerJumpSound);
        }

        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move((playerVelocity + knockback) * Time.deltaTime);
    }

    IEnumerator shoot(){
        isShooting = true;

        audSource.PlayOneShot(playerShootSound);
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(.5f, .5f)), out hit, shootDistance)){
            IDamage damage = hit.collider.GetComponent<IDamage>();

            if(hit.transform != transform && damage != null){
                damage.takeDamage(shootDamage);
            }

            Instantiate(shootEffect, hit.point, hit.transform.rotation);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator shieldCharger(){

        yield return new WaitForSeconds(shieldTimer);
        shieldRecharging = true;
    }

    public void takeDamage(int amount)
    {
        audSource.PlayOneShot(playerHurtSound);
        StartCoroutine(flashDamage());

        if (shieldHP <= 0 || amount < 0){
            HP -= amount;
        }
        else{
            shieldHP -= amount;

            // if player is attacked while their shield is recharging, the charging process should be interrupted
            StopCoroutine(shieldCharger());
            shieldRecharging = false;
            StartCoroutine(shieldCharger());
        }

        if(HP <= 0){
            audSource.PlayOneShot(playerDeathSound);
            // This is where the player dies, Game over screen
            GameManager.instance.youLose();
        }

        updatePlayerUI();
    }

    public void takePhysics(Vector3 amount)
    {
        knockback = amount;
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
        GameManager.instance.shieldHPBar.fillAmount = shieldHP / shieldHPmax;
    }

    IEnumerator flashDamage()
    {
        // show damage flash
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }
    public IEnumerator Damage(float sec, int dmgBoost, GameObject obj)
    {
        isPowerUpCoroutineRunning = true;
        audSource.PlayOneShot(powerupSound);
        GameManager.instance.damageBoostIcon.SetActive(true);
        int orig = shootDamage;
        shootDamage += dmgBoost;
        yield return new WaitForSecondsRealtime(sec);
        GameManager.instance.damageBoostIcon.SetActive(false);
        shootDamage = orig;
        audSource.PlayOneShot(powerdownSound);
        Destroy(obj);
        isPowerUpCoroutineRunning = false;
    }
    public IEnumerator SpeedPowerUp(float sec, float multiplier, GameObject obj)
    {
        isPowerUpCoroutineRunning = true;
        audSource.PlayOneShot(powerupSound);
        GameManager.instance.speedBoostIcon.SetActive(true);
        float orig = playerSpeed;
        float camOrig = cam.fieldOfView;
        playerSpeed *= multiplier;
        cam.fieldOfView = 80;
        yield return new WaitForSeconds(sec);
        GameManager.instance.speedBoostIcon.SetActive(false);
        playerSpeed = orig;
        cam.fieldOfView = camOrig;
        audSource.PlayOneShot(powerdownSound);
        Destroy(obj);
        isPowerUpCoroutineRunning = false;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Scrap"))
        {
            audSource.PlayOneShot(scrapCollectedSound);
        }
    }
}

