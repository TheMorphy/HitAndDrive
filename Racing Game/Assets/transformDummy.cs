using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformDummy : MonoBehaviour
{
    movefoward MoveFoward;

    TrackManager tm;

    [SerializeField]
    List<Rigidbody> limbs;
    Coroutine waitAFrame;
    bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        MoveFoward = FindObjectOfType<movefoward>();
        tm = TrackManager.instance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        

        if(tm.currentlevel < 5 && waitAFrame == null)  
        {
            waitAFrame = StartCoroutine(WaitOneFrame());
        }
        if (!stop)
        {
            transform.position = MoveFoward.transform.position;
        }
        
    }

    private void FixedUpdate()
    {
        if (tm.currentlevel < 5)
        {
            foreach (Rigidbody r in limbs)
            {
                r.mass = 0.5f;
                r.drag = 0.5f;
            }
            enabled = false;
        }
    }

    IEnumerator WaitOneFrame()
    {
        yield return null;
        stop = true;
    }
}
