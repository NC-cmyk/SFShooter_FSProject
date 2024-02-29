using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwap : MonoBehaviour
{
    /* === IMPORTANT ===
     * Levels need to be built or else swapping can't occur
     */

    [Header("--- Level Chooser ---")]
    [Range(0, 4)] [SerializeField] int levelToLoad;
    [SerializeField] bool winCondition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (winCondition)
            {
                YouWonTheGame();
            }
            else
            {
                StartCoroutine(loadScene());
            }
        }
    }

    IEnumerator loadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameManager.instance.sceneNames[levelToLoad]);

        while(!asyncLoad.isDone)
        {
            GameManager.instance.loadingScreen.SetActive(true);
            yield return null;
        }

        GameManager.instance.loadingScreen.SetActive(false);
    }
    void YouWonTheGame()
    {
        StopAllCoroutines();
        GameManager.instance.GameGoalComplete();
    }
}
