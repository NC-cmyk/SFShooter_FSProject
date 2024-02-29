using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpawnerTracker : SpawnerTracker
{
    [Header("--- Ranged Spawner Tracker Components ---")]
    [SerializeField] GameObject[] killSwitches;

    GameObject realKillSwitch;
    KillSwitch killSwScript;

    private void Awake()
    {
        int ksIndex = Random.Range(0, killSwitches.Length);
        realKillSwitch = killSwitches[ksIndex];
        killSwScript = realKillSwitch.GetComponent<KillSwitch>();
        killSwScript.setRealSwitch(true);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
