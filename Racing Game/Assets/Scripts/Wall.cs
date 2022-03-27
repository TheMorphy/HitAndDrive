using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField] float rbVelocityToBreak;

    LevelSystem levelSystem;

    TextMeshProUGUI multiplierText;
    private void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
        multiplierText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void callCarCrash(Collider collision)
    {
        if(!carCollided)
        
        Collide(collision);
    }
    public void Collide(Collider collision)
    {
        if(collision.gameObject.layer == 8 && !carCollided || collision.GetComponent<ExplosiveBarrel>() != null && !carCollided)
        {
            carCollided = true;

            if (multiplierText != null)
            {
                multiplierText.enabled = false;
            }

            if (TrackManager.instance.currentlevel < levelToBreakThrough || levelToBreakThrough == 0 && !TrackManager.instance.fever && collision.GetComponent<ExplosiveBarrel>() == null)
            {
                if (TrackManager.instance.currentlevel - levelToLose <= 0)
                {
                    //Die
                    AudioManager.instance.PlaceSound("Stone_Car", transform.position, null, 1);
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
            if((collision.gameObject.layer == 9 || collision.gameObject.layer == 14 && collision.attachedRigidbody.velocity.magnitude > rbVelocityToBreak) && !carCollided)
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
                disableOverTime(g, 5);
                g.GetComponent<PlaySoundOnTouch>().startDestructed();
                rb.AddExplosionForce(TrackManager.instance.wallDestructionForce, collision.transform.position, 5);
                carCollided = true;
            }
        }

    }

    IEnumerator disableOverTime(GameObject obj, float time)
    {

        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        yield break;
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
