using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{

    [SerializeField] GameObject lsGameobject;
    [SerializeField] GameObject tmGameobject;

    LevelSystem levelSystem;
    TrackManager trackManager;

    movefoward mf;

    private void Start()
    {
        levelSystem = lsGameobject.GetComponent<LevelSystem>();
        trackManager = tmGameobject.GetComponent<TrackManager>();
        mf = FindObjectOfType<movefoward>();

        levelSystem.Multiplier = 0.7f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && trackManager.currentlevel > 9)
        {
            levelSystem.Multiplier += 0.3f;
            mf.MoveSpeed /= 2;
            print(levelSystem.Multiplier);
        }
    }
}
