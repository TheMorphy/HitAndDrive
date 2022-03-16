using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target, camPos;
    [SerializeField] GameObject driver;
    private Vector3 playerOffset;

    private Vector3 currentOffset;
    [SerializeField] public Vector3 offset = Vector3.zero;
    [SerializeField]
    Vector3 feverCamPos, feverCamRot;
    [SerializeField]
    float smoothfever = 0.1f;

    Animator camAnim;

    TrackManager tm;

    DriverFly driverFly;
    private void Start()
    {
        camAnim = GetComponentInChildren<Animator>();
        playerOffset = transform.position - target.position;

        driverFly = FindObjectOfType<DriverFly>();
        tm = FindObjectOfType<TrackManager>();
        currentOffset = playerOffset;
    }

    void Update()
    {
        
        //if (!player.isGrounded() || player.isStunned)
        //{
        //    return;
        //}
        if(target !=null)
        {
            Vector3 newV3 = new Vector3(transform.position.x, target.position.y + currentOffset.y, target.position.z + currentOffset.z);
            transform.position = newV3;


            if (TrackManager.instance.fever)
                camAnim.SetBool("fever", true);
            else
                camAnim.SetBool("fever", false);
        }

        if (driverFly.IsSpawned)
        {
            if (tm.currentlevel < 10)
            {
                target = FindObjectOfType<transformDummy>().transform;
            } else 
                target = FindObjectOfType<movefoward>().transform;
        }

    }
}
