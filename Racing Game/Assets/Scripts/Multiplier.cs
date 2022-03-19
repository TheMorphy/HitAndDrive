using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    LevelSystem levelSystem;
    TrackManager trackManager;
    DriverFly driverFly;

    movefoward mf;

    public static bool hasHitOnce;

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
            hasHitOnce = true;
            if (trackManager.currentlevel > 9)
            {
                mf = FindObjectOfType<movefoward>();
                levelSystem.MultiplierL += 0.3f;
                mf.MoveSpeed -= 5;
            }
        }
    }
}
