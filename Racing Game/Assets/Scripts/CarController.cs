using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody carRB;
    [SerializeField] public Rigidbody sphereRB;

    [HideInInspector]
    public float startSpeed;
    [SerializeField] public float speed;

    [SerializeField] public float nitroSpeed;

    //[SerializeField] float revSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] LayerMask groundLayer;

    private float turnInput;
    private bool isCarGrounded;

    private float normalDrag;
    [SerializeField] float modifiedDrag;

    [SerializeField] float alignToGroundTime;

    [SerializeField] TrailRenderer speedTrail;

    [SerializeField] GameObject levelManager;

    float yRotation, forcedRotation, xStartingPosition;

    LevelSystem levelSystem;

    //DAVID VARIABLES
    public float ForcedRotation { get => forcedRotation; set => forcedRotation = value; }
    public float XStartingPosition { get => xStartingPosition; set => xStartingPosition = value; }
    public float TurnSpeed { get => turnSpeed; set => turnSpeed = value; }

    void Start()
    {
        // Detach Sphere from car
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        normalDrag = sphereRB.drag;

        startSpeed = speed;

        levelSystem = levelManager.GetComponent<LevelSystem>();

        XStartingPosition = transform.position.x;
    }

    void Update()
    {
        // Get Input
        turnInput = Input.GetAxisRaw("Horizontal");

        // Calculate Turning Rotation
        float newRot = turnInput * TurnSpeed * Time.deltaTime;

        if (isCarGrounded)
            transform.Rotate(0, newRot, 0, Space.World);

        if (levelSystem.HasFinished == false)
        {
            yRotation = Mathf.Clamp(yRotation + Input.GetAxis("Horizontal") * TurnSpeed * Time.deltaTime, -30, 30);
        }
        else
        {
            yRotation = Mathf.Clamp((yRotation + forcedRotation) * TurnSpeed * Time.deltaTime, -30, 30);
        }

        transform.eulerAngles = new Vector3(0.0f, yRotation, 0);

        // Set Cars Position to Our Sphere
        transform.position = sphereRB.transform.position;

        // Raycast to the ground and get normal to align car with it.
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 0.5f, groundLayer);

        // Rotate Car to align with ground
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);

        // Calculate Drag
        sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;
    }

    private void FixedUpdate()
    {
        if (isCarGrounded)
            sphereRB.AddForce(transform.forward * speed, ForceMode.Acceleration); // Add Movement
        else
            sphereRB.AddForce(transform.up * -200f); // Add Gravity

        carRB.MoveRotation(transform.rotation);
    }

    /*public IEnumerator Acceleration()
    {
        speedTrail.emitting = true;
        speed = nitroSpeed;

        yield return new WaitForSeconds(1.5f);

        speed = startSpeed;
        speedTrail.emitting = false;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("stop"))
        {
            //sphereRB.constraints = RigidbodyConstraints.FreezePositionX;
        }
    }
}