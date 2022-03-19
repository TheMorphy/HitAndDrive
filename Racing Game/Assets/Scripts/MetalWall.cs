using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalWall : MonoBehaviour
{
    [SerializeField]
    float impactForce, spin;
    [SerializeField]
    int levelToLose;

    Rigidbody rb;
    bool collided = false;
    [SerializeField] GameObject notDestructed, destructed;

    LevelSystem levelSystem;

    private void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == 8 && !collided && levelSystem.HasFinished == false)
        {
            if (TrackManager.instance.currentlevel - levelToLose <= 0)
            {
                //Die
                TrackManager.instance.PlayerDie(other.transform.position);
                collided = true;
                return;
            }
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            Physics.IgnoreLayerCollision(gameObject.layer, 0);
            Vector3 force = (TrackManager.instance.transform.forward * 3 + Vector3.up * 0.5f + TrackManager.instance.transform.right * MoveDir()) * impactForce;
            rb.AddForce(force, ForceMode.Force);
            rb.AddTorque(Random.Range(0f, 1f) * spin, Random.Range(0f, 1f) * spin, Random.Range(0f, 1f) * spin, ForceMode.VelocityChange);
            TrackManager.instance.changeLevel(-levelToLose, "-", true);
            collided = true;
            notDestructed.SetActive(false);
            destructed.SetActive(true);
            foreach (Collider c in GetComponents<Collider>())
            {
                c.isTrigger = true;
            }
        }
    }

    int MoveDir()
    {
        if (TrackManager.instance.transform.position.x - transform.position.x >= 0)
            return -1;
        else
            return 1;
    }
}
