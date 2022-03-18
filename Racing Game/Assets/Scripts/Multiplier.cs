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

        levelSystem.Multiplier = 0.7f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && trackManager.currentlevel > 9)
        {
            mf = FindObjectOfType<movefoward>();
            levelSystem.Multiplier += 0.3f;
            mf.MoveSpeed -= 5;
        }
    }
}
