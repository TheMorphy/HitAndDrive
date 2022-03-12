using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalWall : MonoBehaviour
{
    [SerializeField]
    float impactForce, spin;
    [SerializeField]
    int levelsToLose;

    Rigidbody rb;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            Physics.IgnoreLayerCollision(gameObject.layer, 0);
            Vector3 force = (TrackManager.instance.transform.forward + Vector3.up * 0.2f + TrackManager.instance.transform.right * MoveDir()) * impactForce;
            rb.AddForce(force, ForceMode.Force);
            rb.AddTorque(Random.Range(0f, 1f) * spin, Random.Range(0f, 1f) * spin, Random.Range(0f, 1f) * spin, ForceMode.VelocityChange);
            TrackManager.instance.changeLevel(-levelsToLose, "-", true);
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
