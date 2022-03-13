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
                wall.callCarCrash(collision.collider);
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

}
