using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverFly : MonoBehaviour
{
    [SerializeField] Transform prefab;
    bool isSpawned;
    bool isBlocked;

    TrackManager tm;
    CarController carScript;

    float wallsToBreak;

    public bool IsSpawned { get => isSpawned; set => isSpawned = value; }
    public bool IsBlocked { get => isBlocked; set => isBlocked = value; }
    public float WallsToBreak { get => wallsToBreak; set => wallsToBreak = value; }

    private void Start()
    {
        tm = FindObjectOfType<TrackManager>();
        carScript = FindObjectOfType<CarController>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
        {
            carScript.startSpeed = 0;
            if (!IsSpawned) 
            {
                IsSpawned = true;
                IsBlocked = true;
                if (tm.currentlevel > 9)
                {
                    WallsToBreak = Mathf.FloorToInt(tm.currentlevel / 10);
                    print(WallsToBreak);
                    Instantiate(prefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
