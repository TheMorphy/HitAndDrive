using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Wall Properties")]
    [SerializeField]
    int levelToBreakThrough = 0;    
    enum wallModes { Splattering, Physically, consistant }
    [SerializeField]
    wallModes wallMode;

    [SerializeField]
    int levelToLose = 5;
    public string partContactSoundHeavy, partContactSoundMedium, partContactSoundLight;
    public float nonSoundlimit, lightSoundVelLimit, MediumSoundLimit;
    public bool velocityVolumeImpact;

    [SerializeField]
    List<GameObject> parts = new List<GameObject>();

    bool carCollided = false;
    private void Start()
    {
        
    }

    public void callCarCrash(Collider collision)
    {
        if(!carCollided)
        
        Collide(collision);
    }
    private void Collide(Collider collision)
    {
        if(collision.gameObject.layer == 8 && !carCollided)
        {
            carCollided = true;
            AudioManager.instance.PlaceSound("Stone_Car", transform.position, null, 1);

            if (TrackManager.instance.currentlevel < levelToBreakThrough || levelToBreakThrough == 0 && !TrackManager.instance.fever)
            {
                if (TrackManager.instance.currentlevel - levelToLose < 0)
                {
                    //Die
                    TrackManager.instance.PlayerDie(collision.transform.position);
                }
                else
                TrackManager.instance.changeLevel(-levelToLose, "", true);
                
            }
            foreach (GameObject g in parts)
                {
                    Rigidbody rb = g.GetComponent<Rigidbody>();
                    Vector3 dirToMove = (collision.ClosestPoint(g.transform.position) - g.transform.position).normalized;
                    bool rightOrLeft = Vector3.SignedAngle(collision.transform.forward, dirToMove, Vector3.up) < 0;
                    if (rightOrLeft)
                        dirToMove -= collision.transform.right * 0.5f;
                    else
                        dirToMove += collision.transform.right * 0.5f;
                    dirToMove -= collision.transform.forward * 0.5f;
                    rb.isKinematic = false;
                    StartCoroutine(Explode(rb, dirToMove));
                    Destroy(g, 5f);
                    g.GetComponent<PlaySoundOnTouch>().startDestructed();
                    //rb.AddExplosionForce(TrackManager.instance.wallDestructionForce, contact, 5);

                }
            
            
        }
        else
            if((collision.gameObject.layer == 9 || collision.gameObject.layer == 14 && collision.attachedRigidbody.velocity.magnitude > 10) && !carCollided)
            {
            foreach (GameObject g in parts)
            {
                Rigidbody rb = g.GetComponent<Rigidbody>();
                Vector3 dirToMove = (collision.ClosestPoint(g.transform.position) - g.transform.position).normalized;
                bool rightOrLeft = Vector3.SignedAngle(collision.transform.forward, dirToMove, Vector3.up) < 0;
                if (rightOrLeft)
                    dirToMove -= collision.transform.right * 0.5f;
                else
                    dirToMove += collision.transform.right * 0.5f;
                dirToMove -= collision.transform.forward * 0.5f;
                rb.isKinematic = false;
                StartCoroutine(Explode(rb, dirToMove, 5));
                //Destroy(g, 5f);
                g.GetComponent<PlaySoundOnTouch>().startDestructed();
                rb.AddExplosionForce(TrackManager.instance.wallDestructionForce, collision.transform.position, 5);
                carCollided = true;
            }
        }

    }

    IEnumerator Explode(Rigidbody rb, Vector3 position, float multiplicator = 1)
    {
        yield return new WaitForSeconds(TrackManager.instance.wallExplodeDelay);
        rb.AddForce(-position * TrackManager.instance.wallDestructionForce * multiplicator, ForceMode.Force);
       // rb.AddExplosionForce(TrackManager.instance.wallDestructionForce, position, 7);
    }

  /*  private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == 8)
        {
            if(TrackManager.instance.currentlevel >= levelToBreakThrough)
            {
                foreach (GameObject g in parts)
                {
                    Rigidbody rb = g.GetComponent<Rigidbody>();

                    //rb.isKinematic = false;
                    rb.AddExplosionForce(TrackManager.instance.wallDestructionForce, collision.transform.position, 7);

                }
            }
        }
    }*/

}
