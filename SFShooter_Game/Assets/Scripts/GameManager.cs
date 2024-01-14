using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    public GameObject player;

    public bool isPaused;
    int enemyCount;
    //Awake used to initialize the game manager first 
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            PausedState();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
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
    public void GameGoalUpdate(int amount)
    {
        enemyCount += amount;
        if(enemyCount <= 0)
        {
            PausedState();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }
    }
}
