using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTouch : MonoBehaviour
{
    [SerializeField]
    Wall wall;
    Rigidbody rb;
    float velMagnitude;
    float preVelMag;
    float endMagnitude;

    public void startDestructed()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(destructed());
    }
     IEnumerator destructed()
    {
        bool stop = false;
        //print("hmm");
        preVelMag = rb.velocity.magnitude;
        velMagnitude = rb.velocity.magnitude;
        while (!stop)
        {
            //print("dasdas" + velMagnitude);
            yield return new WaitForFixedUpdate();
            velMagnitude = rb.velocity.magnitude;
            if(velMagnitude + .1f < preVelMag)
            {
                endMagnitude = preVelMag;
            }
            else
            {
                preVelMag = velMagnitude;
            }
            
        }
        

        
    }
    public void getHittedbyExplosion(Collider collision)
    {
        print("dadasdaDSdsd");
        wall.callCarCrash(collision);
        rb = GetComponent<Rigidbody>();
        rb.AddForceAtPosition((transform.position - collision.transform.position).normalized * 24, collision.transform.position, ForceMode.Force);

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        int layer = collision.gameObject.layer;
        rb = GetComponent<Rigidbody>();
        string soundName;
        float costomVolume = 0;
        switch (layer)
        {
            case 8:
            case 9:
            case 14:
                //print(collision.gameObject);
                wall.callCarCrash(collision.collider);
                rb.AddForceAtPosition((transform.position - collision.GetContact(0).point).normalized * TrackManager.instance.wallDestructionForce, collision.GetContact(0).point, ForceMode.Force);
                break;
            case 0:
                if (endMagnitude < wall.nonSoundlimit)
                {
                    costomVolume = 0.02f;
                    soundName = wall.partContactSoundLight;
                }
                else if (endMagnitude < wall.lightSoundVelLimit)
                    soundName = wall.partContactSoundLight;
                else if (endMagnitude < wall.MediumSoundLimit)
                    soundName = wall.partContactSoundMedium;
                else
                    soundName = wall.partContactSoundHeavy;


                print(soundName);
                AudioManager.instance.PlaceSound(soundName, collision.GetContact(0).point, null, 1, costomVolume);
                break;
        }
        preVelMag = 0;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            rb.AddForce((transform.position - collision.GetContact(0).point).normalized * 2, ForceMode.Force);

        }
    }

}
