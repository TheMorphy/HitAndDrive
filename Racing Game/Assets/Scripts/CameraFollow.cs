using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target, camPos;
    private Vector3 playerOffset;

    private Vector3 currentOffset;
    [SerializeField] public Vector3 offset = Vector3.zero;
    [SerializeField]
    Vector3 feverCamPos, feverCamRot;
    [SerializeField]
    float smoothfever = 0.1f;

    Animator camAnim;
    private void Start()
    {
        camAnim = GetComponentInChildren<Animator>();
        playerOffset = transform.position - target.position;

        currentOffset = playerOffset;
    }

    void Update()
    {
        
        //if (!player.isGrounded() || player.isStunned)
        //{
        //    return;
        //}
        Vector3 newV3 = new Vector3(transform.position.x, target.position.y + currentOffset.y, target.position.z + currentOffset.z);
        transform.position = newV3;
        

        if(TrackManager.instance.fever)
            camAnim.SetBool("fever", true);       
        else
            camAnim.SetBool("fever", false);

    }
}
