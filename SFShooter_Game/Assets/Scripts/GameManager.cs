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

    public PlayerController playerScript;
    public GameObject player;
    public Image playerHPBar;
    public GameObject playerDamageFlash;
    public GameObject playerSpawnPosition;
    public ScrapTracker scrapTracker;
    public Image shieldHPBar;

    public bool isPaused;
    int enemyCount;
    int totalScrap;
    
    //Awake used to initialize the game manager first 
    void Awake()
    {
        instance = this;
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
    }

    public void PausedState()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnpausedState()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
    public void GameGoalComplete()
    {
        PausedState();
        activeMenu = winMenu;
        activeMenu.SetActive(true);
    }
    public void youLose(){
        PausedState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    void CollectedScrap()
    {
        // Check if the instance is not null
        if (scrapTracker != null)
        {
            // Use scrapTracker to access its methods or properties
             totalScrap = scrapTracker.TotalScrap;
            // ... (other actions)
        }
        else
        {
            Debug.LogError("ScrapTracker instance not found!");
        }
    }
}

