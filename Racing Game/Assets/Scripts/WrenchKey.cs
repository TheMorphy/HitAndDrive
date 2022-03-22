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
    [SerializeField] ParticleSystem collectParticle;

    void Collect()
    {
        if(!collected)
        {
            anim.Play("WrenchKeyCollect");
            //TrackManager.instance.getCurrentCarLevel().levelToReach = TrackManager.instance.currentlevel;
            TrackManager.instance.changeLevel(levelToAdd);
            collected = true;
            FindObjectOfType<AudioManager>().Play("Collect");
            ParticleSystem collect = Instantiate(collectParticle, transform.position, Quaternion.identity);
            collect.Play();
            StartCoroutine(DestroyParticle(collect));
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

    IEnumerator DestroyParticle(ParticleSystem particle)
    {
        yield return new WaitForSeconds(0.4f);

        Destroy(particle);
    }
}
