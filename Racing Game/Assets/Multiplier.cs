using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{

    [SerializeField] GameObject lsGameobject;
    [SerializeField] GameObject tmGameobject;

    LevelSystem levelSystem;
    TrackManager trackManager;

    private void Start()
    {
        levelSystem = lsGameobject.GetComponent<LevelSystem>();
        trackManager = tmGameobject.GetComponent<TrackManager>();
        levelSystem.Multiplier = 1.0f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && trackManager.currentlevel > 9)
        {
            levelSystem.Multiplier += 0.3f;
        }
    }
}
