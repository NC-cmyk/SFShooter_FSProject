using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.UnpausedState();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.UnpausedState();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void respawnplayer()
    {
        GameManager.instance.playerScript.respawn();
        GameManager.instance.UnpausedState(); 
    }
}


