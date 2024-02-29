using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    public string _newGameLevel;
    public string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    public void Start()
    {
        // if the player went back to the main menu from the game
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
        }
    }

    public void NewGameDialogYes()
    {
        // just to make sure
        PlayerPrefs.DeleteKey("SavedLevel");

        StartCoroutine(loadScene(_newGameLevel));
    }

    public void LoadGameDialogYes()
    {
        bool hasSavedLevel = PlayerPrefs.HasKey("SavedLevel");
        if (hasSavedLevel)
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            StartCoroutine(loadScene(levelToLoad));
        }
        else if(!hasSavedLevel)
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator loadScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameManager.instance.playerScript.respawn();
    }
}

   