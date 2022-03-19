using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int lvl = 1;
    [SerializeField] int lvlToKill = 0;
    [SerializeField]
    float startHitRange;
    Rigidbody rb;
    [SerializeField] GameObject canvas;
    [SerializeField] Rigidbody pelvisRB;
    [SerializeField] List<Rigidbody> bodyParts = new List<Rigidbody>();
    [HideInInspector] public bool collidedOnce = false;
    float force;

    [SerializeField] float forwardForce;
    [SerializeField] float upwardsForce;
    [SerializeField] float sideForce;

    private void Start()
    {
        

        if(pelvisRB == null)
        {
            pelvisRB = this.GetComponent<Rigidbody>();
        }
        foreach(BoxCollider b in GetComponents<BoxCollider>())
        {
            b.enabled = true;
        }
        
        force = TrackManager.instance.carCrashPower;
    }
    private void LateUpdate()
    {
        if(TrackManager.instance.car != null)
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
            if(canvas == null)
            transform.parent.transform.parent.transform.parent.GetChild(2).GetComponent<Canvas>().enabled = false;
            else
            {
                canvas.GetComponent<Canvas>().enabled = false;
            }

            rb = GetComponent<Rigidbody>();

            animator.enabled = false;
            rb.isKinematic = false;
            foreach (Rigidbody r in bodyParts)
            {
                r.isKinematic = false;
            }

            //Update the killbar       
            TrackManager.instance.killValue += 1;

            if (!collidedOnce)
            {
                TrackManager.instance.changeLevel(lvl);
                FindObjectOfType<AudioManager>().Play("Hit");
                //Apply force
                Vector3 dirToMove = (other.transform.position - transform.position).normalized;
                bool rightOrLeft = Vector3.SignedAngle(other.transform.forward, dirToMove, Vector3.up) < 0;
                Vector3 moveDir;
                if(rightOrLeft)
                {
                    moveDir = (other.transform.forward * forwardForce) + (other.transform.up * upwardsForce + other.transform.right * sideForce);
                }else
                {
                    moveDir = (other.transform.forward * forwardForce) + (other.transform.up * upwardsForce) + (other.transform.right * -sideForce);
                }
                Vector3 forceDir = moveDir;
                pelvisRB.mass = 1f;
                pelvisRB.AddForce(forceDir * force, ForceMode.Impulse);
                
                collidedOnce = true;

                Destroy(gameObject, 5);
            }

            //StartCoroutine(DestroySelf());
        }
        else if(other.gameObject.layer == 8)
        {
            // transform.parent.parent.parent.GetComponent<Animator>().SetFloat("randomHit", System.Convert.ToInt16(Random.Range(2, 3)));
            TrackManager.instance.PlayerDie(Vector3.zero);
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

    

    //IEnumerator DestroySelf()
    //{
    //    yield return new WaitForSeconds(5f);

    //    Destroy(gameObject.transform.parent.transform.parent.transform.parent.gameObject);
    //}
}
