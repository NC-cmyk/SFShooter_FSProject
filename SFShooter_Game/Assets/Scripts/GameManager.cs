using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("--- Menus ---")]
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] AudioSource menuSFX;

    [Header("--- Level Scenes ---")]
    public string[] sceneNames;

    [Header("--- Player Components ---")]
    public PlayerController playerScript;
    public GameObject player;
    public Image playerHPBar;
    public Image shieldHPBar;
    public GameObject playerDamageFlash;
    public GameObject playerSpawnPosition;

    [Header("--- Scrap Tracker ---")]
    public ScrapTracker scrapTracker;

    [Header("--- Boss Components ---")]
    [SerializeField] GameObject bossHP;
    public GameObject boss;
    public BossAI bossScript;
    public Image bossHPBar;
    public bool bossActive;

    [Header("--- Pause State ---")]
    public bool isPaused;
    
    //Awake used to initialize the game manager first 
    void Awake()
    {
        instance = this;

        sceneNames = new string[] { "Introduction Level", "Level 1", "Level 2", "Level 3", "Level 4" };

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        if (Time.timeScale == 0)
        {
            UnpausedState();
        }

        scrapTracker = ScrapTracker.instance;

        // so menu sfx can still play
        menuSFX.ignoreListenerPause = true;
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
        isPaused = true;
        AudioListener.pause = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnpausedState()
    {
        isPaused = false;
        AudioListener.pause = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // active menu does not exist when loading back into the game
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }
    public void GameGoalComplete()
    {
        playerScript.audSource.PlayOneShot(playerScript.playerWinSound);
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

