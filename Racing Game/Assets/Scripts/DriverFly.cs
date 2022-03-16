using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverFly : MonoBehaviour
{
    [SerializeField] Transform prefab;
    bool isSpawned;
    bool isBlocked;

    TrackManager tm;

    public bool IsSpawned { get => isSpawned; set => isSpawned = value; }
    public bool IsBlocked { get => isBlocked; set => isBlocked = value; }

    private void Start()
    {
        tm = FindObjectOfType<TrackManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
        {
            if (!IsSpawned) 
            {
                IsSpawned = true;
                IsBlocked = true;
                if (tm.currentlevel > 9)
                {
                    Instantiate(prefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
