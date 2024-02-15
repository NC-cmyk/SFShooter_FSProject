using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    // REPURPOSE FOR OTHER TRACKING
    // POTENTIALLY BOSS TRACKING?

    public static CombatManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
}
