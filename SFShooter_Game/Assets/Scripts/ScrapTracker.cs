using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrapTracker : MonoBehaviour
{
    [SerializeField] TMP_Text scrapTrackText;

    public static ScrapTracker instance;

    public int totalScrap;
    public int requiredScrapCount;
    
    public int TotalScrap => totalScrap;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        scrapTrackText.text = totalScrap + "/" + requiredScrapCount + " Scrap Collected";
    }

    public void CollectScrap()
    {
        totalScrap++;
        CheckGoals();
    }
    private void CheckGoals()
    {
        // Check if the total scrap count equals the required amount for escape
        if (totalScrap >= requiredScrapCount)
        {
            // Trigger escape event or subgoal completion
            GameManager.instance.GameGoalComplete();  
        }
    }
}
