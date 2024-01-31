using UnityEngine;

public class ScrapTracker : MonoBehaviour
{
    public static ScrapTracker instance;

    private int totalScrap;
    public int requiredScrapCount; 

    public int TotalScrap => totalScrap;

    private void Awake()
    {
        instance = this;
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
            GameManager.instance.GameGoalUpdate(1);  
        }
    }
}
