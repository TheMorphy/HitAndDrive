using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    LevelSystem levelSystem;
    TrackManager trackManager;
    DriverFly driverFly;

    movefoward mf;

    private void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
        trackManager = FindObjectOfType<TrackManager>();
        driverFly = FindObjectOfType<DriverFly>();

        levelSystem.MultiplierL = 0.7f;
    }

    private void Update()
    {
        //print(hasHitOnce);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (trackManager.currentlevel > 4)
            {
                mf = FindObjectOfType<movefoward>();
                levelSystem.MultiplierL += 0.3f;
                mf.MoveSpeed -= 2.5f;
                //Fix Level 5 bug
                if (trackManager.currentlevel == 5)
                {
                    trackManager.currentlevel = 6;
                }
            }
        }
    }
}
