using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    float range, force;
    [SerializeField]
    GameObject explosionParticle;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        




    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Explode()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        foreach (Collider c in Physics.OverlapSphere(transform.position, range))
        {
            // print(c.name);
          //  if (c.GetComponent<PlaySoundOnTouch>() != null)
           // {
          //      c.GetComponent<PlaySoundOnTouch>().getHittedbyExplosion(GetComponent<Collider>());
         //   }
            if (c.attachedRigidbody != null && c.GetComponent<PlaySoundOnTouch>() == null)
            {
                c.attachedRigidbody.AddForceAtPosition((c.transform.position -transform.position).normalized * force, transform.position, ForceMode.Impulse);
                //c.attachedRigidbody.AddExplosionForce(force, transform.position, range);
                //print(c.name);
            }
            
            c.SendMessage("getHittedbyExplosion", GetComponent<Collider>());

           // GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, range);
        }
        GetComponent<Collider>().isTrigger = true;
        enabled = false;
        Destroy(transform.GetChild(0).gameObject);

    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Explode();
        }
        else if(collision.gameObject.layer == 9 || collision.gameObject.layer == 14)
        {
            if (collision.rigidbody.velocity.magnitude > 4)
            {
                Explode();
            }
        }

    }
}
