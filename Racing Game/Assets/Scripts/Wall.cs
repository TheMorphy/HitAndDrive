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
    public string soundName;
    public bool velocityVolumeImpact;

    [SerializeField]
    List<GameObject> parts = new List<GameObject>();


    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == 8)
        {
            if (TrackManager.instance.currentlevel < levelToBreakThrough || levelToBreakThrough == 0 && !TrackManager.instance.fever)
            {
                if (TrackManager.instance.currentlevel - levelToLose < 0)
                {
                    //Die
                    TrackManager.instance.PlayerDie(collision.transform.position);
                }
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
                    //rb.AddExplosionForce(TrackManager.instance.wallDestructionForce, contact, 5);

                }
            
            
        }
    }

    IEnumerator Explode(Rigidbody rb, Vector3 position)
    {
        yield return new WaitForSeconds(TrackManager.instance.wallExplodeDelay);
        rb.AddForce(-position * TrackManager.instance.wallDestructionForce, ForceMode.Force);
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
