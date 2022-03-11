using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField]
    Wall groupHolder;

    private void OnCollisionEnter(Collision collision)
    {
        if(groupHolder.velocityVolumeImpact)
        {
            float vol = GetComponent<Rigidbody>().velocity.magnitude;
            //print(vol);
            AudioManager.instance.PlaceSound(groupHolder.soundName, collision.GetContact(0).point, null, 0.8f, 1);
        }

    }

}
