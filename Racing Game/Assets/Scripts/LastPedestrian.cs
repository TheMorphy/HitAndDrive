using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPedestrian : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int lvl = 1;
    [SerializeField] int lvlToKill = 0;
    [SerializeField]
    float startHitRange;
    Rigidbody rb;

    CarController carScript;

    [HideInInspector] public bool collidedOnce = false;
    float force;
    private void Start()
    {

        carScript = FindObjectOfType<CarController>();

        foreach (BoxCollider b in GetComponents<BoxCollider>())
        {
            b.enabled = true;
        }

        force = TrackManager.instance.carCrashPower;
    }
    private void LateUpdate()
    {
        if (TrackManager.instance.car != null)
        {
            float range = Vector3.Distance(transform.position, TrackManager.instance.car.position);
            if (range <= startHitRange)
            {
                transform.parent.parent.parent.GetComponent<Animator>().SetBool("hit", true);

            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && TrackManager.instance.currentlevel >= lvlToKill)
        {
            transform.parent.transform.parent.transform.parent.GetChild(2).GetComponent<Canvas>().enabled = false;

            rb = GetComponent<Rigidbody>();

            animator.enabled = false;
            rb.isKinematic = false;

            //Update the killbar       
            TrackManager.instance.killValue += 1;

            if (!collidedOnce)
            {
                TrackManager.instance.RemoveLevel(lvl);
                FindObjectOfType<AudioManager>().Play("Hit");
                //Apply force
                Vector3 dirToMove = (other.transform.position - transform.position).normalized;
                bool rightOrLeft = Vector3.SignedAngle(other.transform.forward, dirToMove, Vector3.up) < 0;
                Vector3 moveDir;
                if (rightOrLeft)
                {
                    moveDir = other.transform.forward + (other.transform.up + other.transform.right * 0.5f);
                }
                else
                {
                    moveDir = other.transform.forward + (other.transform.up * 0.5f) + (other.transform.right * -0.5f);
                }
                Vector3 forceDir = moveDir;
                rb.mass = 1f;
                rb.AddForce(forceDir * force, ForceMode.Impulse);

                collidedOnce = true;
            }

            //StartCoroutine(DestroySelf());
        }
        else if (other.gameObject.layer == 8)
        {
            Debug.Log("Test");
            carScript.speed = 0;
            carScript.nitroSpeed = 0;
            carScript.startSpeed = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && !collidedOnce && rb != null)
        {
            Vector3 forceDir = collision.transform.forward + collision.transform.up;
            rb.mass = 1f;
            rb.AddForce(forceDir * force, ForceMode.Impulse);
        }
    }
}
