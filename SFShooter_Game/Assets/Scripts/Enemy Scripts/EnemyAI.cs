using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("--- Components ---")]
    [SerializeField] Renderer model;  // change back to SerializeField when getters/setters are added
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform headPos; // head position for fov
    [SerializeField] AudioSource audSource;

    [Header("--- General Stats ---")]
    [SerializeField] int HP;
    [Range(20, 180)] [SerializeField] int fov; // field of view
    [Range(1, 10)] [SerializeField] int rotateSpeed;
    [Range(1, 10)] [SerializeField] int animTransSpeed;
    [Range(5, 20)] [SerializeField] int roamDistance;
    [Range(1, 5)] [SerializeField] int roamPauseTimer;

    [Header("----- Audio Clips -----")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip walkSound;

    protected bool playerInRange;
    protected Vector3 playerDir; // player direction
    protected float angleToPlayer;

    bool destChosen; // destination chosen
    Vector3 startingPos; // starting position
    protected float stoppingDistOrig; // stopping distance original
    protected bool gettingDestroyed; // prevent agent from setting destination

    bool isDying; // prevent death sound from being interrupted

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // default audio clip should be walking audio clip
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // destroys minion if the boss dies and theyre still alive
        if (gameObject.tag == "Minion" && !GameManager.instance.bossActive)
        {
            gettingDestroyed = true;
            Destroy(gameObject);
        }

        // updates the movement animation for enemies depending on their current speed
        float animSpeed = agent.velocity.normalized.magnitude;
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), animSpeed, Time.deltaTime * animTransSpeed));

        // movement will be in the child Update() functions
    }

    protected virtual bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        // debug
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer < fov)
            {
                // passing this check means the player is within vision cone
                // therefore they should begin following the player as long as they are within sight
                agent.SetDestination(GameManager.instance.player.transform.position);

                agent.stoppingDistance = stoppingDistOrig;

                // rotation and attacking code will be within child scripts

                return true;
            }
        }

        return false;
    }

    protected void faceTarget()
    {
        // rotation to look towards player
        Quaternion rotation = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        
        // updates rotation smoothly instead of making the enemy snap its rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
    }

    protected IEnumerator roam()
    {
        agent.stoppingDistance = 0;
        if (agent.remainingDistance < 0.05f && !destChosen)
        {
            destChosen = true;

            // stop walking sfx
            if (destChosen)
            {
                audSource.Stop();
            }

            yield return new WaitForSeconds(roamPauseTimer);

            Vector3 randomPos = Random.insideUnitSphere * roamDistance;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            // play walking sfx
            if (!audSource.isPlaying)
            {
                audSource.Play();
            }

            destChosen = false;
        }
    }

    public virtual void takeDamage(int amount)
    {
        // hurt sound effect
        audSource.PlayOneShot(hurtSound);

        HP -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            if(gameObject.tag == "Minion")
            {
                GameManager.instance.bossScript.updateMinionCount(-1);
            }

            if (!isDying)
            {
                StartCoroutine(playDeathSound());
            }
        }
    }
    IEnumerator playDeathSound()
    {
        isDying = true;

        // stop any other sounds that are playing
        if (audSource.isPlaying)
        {
            audSource.Stop();
        }
        audSource.clip = deathSound;
        audSource.Play();

        yield return new WaitForSeconds(deathSound.length);
    }

    IEnumerator flashRed()
    {
        // may be removed once animations are implemented
        Color ogColor = model.material.color;
        
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = ogColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    protected int getHP() { return HP; }
    protected Renderer getModel() { return model; }
    protected NavMeshAgent getAgent() { return agent; }
    protected Transform getHeadPos() { return headPos; }
    protected Animator getAnimator() { return animator; }
    protected AudioSource getAudSource() { return audSource; }
    protected AudioClip getHurtSFX() { return hurtSound; }
}
