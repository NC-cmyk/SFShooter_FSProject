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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(loadScene());
        }
    }

    IEnumerator loadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameManager.instance.sceneNames[levelToLoad]);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        GameManager.instance.playerScript.respawn();
        // However doing this would require the player needing a way to retain their upgrades
        // If upgrades are still getting implemented
    }
}
