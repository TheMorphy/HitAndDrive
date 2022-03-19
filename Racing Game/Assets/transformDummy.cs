using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformDummy : MonoBehaviour
{
    movefoward MoveFoward;

    TrackManager tm;

    // Start is called before the first frame update
    void Start()
    {
        MoveFoward = FindObjectOfType<movefoward>();
        tm = FindObjectOfType<TrackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = MoveFoward.transform.position;

        if(tm.currentlevel < 10 && Multiplier.hasHitOnce == true)
        {
           gameObject.GetComponent<transformDummy>().enabled = false;
        }
    }
}
