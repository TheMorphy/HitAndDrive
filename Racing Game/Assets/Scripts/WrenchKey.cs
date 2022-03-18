using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchKey : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    int levelToAdd = 1;
    bool collected = false;

    void Collect()
    {
        if(!collected)
        {
            anim.Play("WrenchKeyCollect");
            //TrackManager.instance.getCurrentCarLevel().levelToReach = TrackManager.instance.currentlevel;
            TrackManager.instance.changeLevel(levelToAdd);
            collected = true;
            FindObjectOfType<AudioManager>().Play("Collect");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            Collect();
        }
    }

    public void endCollect()
    {
        Destroy(gameObject);
    }
}
