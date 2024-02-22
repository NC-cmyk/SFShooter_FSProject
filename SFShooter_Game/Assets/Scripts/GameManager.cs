using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject bossHP;

    public string[] sceneNames;

    public PlayerController playerScript;
    public GameObject player;
    public Image playerHPBar;
    public Image shieldHPBar;
    public GameObject playerDamageFlash;
    public GameObject playerSpawnPosition;

    public ScrapTracker scrapTracker;

    
    public GameObject boss;
    public BossAI bossScript;
    public Image bossHPBar;
    public bool bossActive;


    public bool isPaused;
    
    //Awake used to initialize the game manager first 
    void Awake()
    {
        instance = this;

        sceneNames = new string[] { "Introduction Level", "Level 1", "Level 2", "Level 3", "Level 4" };

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        scrapTracker = ScrapTracker.instance;
    }

    void Update() 
    {
        if(Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            PausedState();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
        
        if (playerScript != null)
        {
            playerScript.updatePlayerUI();
        }

        if(bossActive && !bossHP.activeSelf)
        {
            bossHP.SetActive(true);
            boss = GameObject.FindGameObjectWithTag("Boss");
            bossScript = boss.GetComponent<BossAI>();
        }
        else if(!bossActive && bossHP.activeSelf){
            bossHP.SetActive(false);
        }
    }

    public void PausedState()
    {
        isPaused = !isPaused;
        AudioListener.pause = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnpausedState()
    {
        isPaused = !isPaused;
        AudioListener.pause = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
    public void GameGoalComplete()
    {
        playerScript.audSource.PlayOneShot(playerScript.playerWinSound, playerScript.winSoundVol);
        PausedState();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }
    public void youLose(){
        PausedState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
}

