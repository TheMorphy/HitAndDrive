using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverFly : MonoBehaviour
{
    [SerializeField] Transform prefab;
    bool isSpawned;

    public bool IsSpawned { get => isSpawned; set => isSpawned = value; }

    //public GameObject spawnPoint;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
        {
            if (!IsSpawned) 
            {
                IsSpawned = true;
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }
    }
}
