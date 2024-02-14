using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpawnerTracker : SpawnerTracker
{
    [Header("--- Ranged Spawner Tracker Components ---")]
    [SerializeField] GameObject killSwitch;

    KillSwitch killSwScript;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        killSwScript = killSwitch.GetComponent<KillSwitch>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (killSwScript.getHit())
        {
            for(int i = getTrackerTransform().childCount - 1; i > -1; i--)
            {
                getTrackerTransform().GetChild(i).GetComponent<RangedEnemyAI>().setShutdown(true);
            }
        }
    }
}
