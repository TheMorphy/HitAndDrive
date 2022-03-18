using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public enum carType { Car, MotorCycle }

    public carType currentCarType;
    [SerializeField] GameObject motorBike;
    bool isleaning;

    [SerializeField] bool isUsingSteeringWheel;

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

    float yRotation, forcedRotation, xStartingPosition, motorCycleyRotation, motorCycleLean, motorCycleTemp;

    private float inputHorizontal;

    public string horizontalAxis = "Horizontal";

    LevelSystem levelSystem;

    //DAVID VARIABLES
    public float ForcedRotation { get => forcedRotation; set => forcedRotation = value; }
    public float XStartingPosition { get => xStartingPosition; set => xStartingPosition = value; }
    public float TurnSpeed { get => turnSpeed; set => turnSpeed = value; }
    public bool IsUsingSteeringWheel { get => isUsingSteeringWheel; set => isUsingSteeringWheel = value; }

    void Start()
    {
        // Detach Sphere from car
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        normalDrag = sphereRB.drag;

        startSpeed = speed;

        levelSystem = levelManager.GetComponent<LevelSystem>();

        XStartingPosition = transform.position.x;
        StartCoroutine(leaningCheck());
    }

    IEnumerator leaningCheck()
    {
        while (gameObject.activeSelf)
        {
            float yrot1 = yRotation;
            float yrot2 = yRotation;
            isleaning = yrot1 != yrot2;
            yield return null;
        }
    }

    void Update()
    {
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
        if(Mathf.Abs(forcedRotation) < 0.2f)
        {
            forcedRotation = 0;
        }

        forcedRotation = transform.position.x - TrackManager.instance.transform.position.x;

        if (!IsUsingSteeringWheel)
        {
            // Get Input
            turnInput = Input.GetAxisRaw("Horizontal");

            // Calculate Turning Rotation
            //float newRot = turnInput * TurnSpeed * Time.fixedDeltaTime;

            if (levelSystem.HasFinished == false)
            {
                float raw = Input.GetAxisRaw("Horizontal") * turnSpeed * Time.fixedDeltaTime * 11;
                yRotation = Mathf.Lerp(yRotation, raw, 0.1f);
            }
            else
            {
                float raw = Mathf.Clamp(Mathf.Clamp(-forcedRotation/2,-1, 1) * turnSpeed * Time.fixedDeltaTime * 11, -30, 30);
                yRotation = Mathf.Lerp(yRotation, raw, 0.5f);
            }
            if (isCarGrounded)
            {

                switch (currentCarType)
                {
                    case carType.Car:
                        //motorCycleyRotation = Mathf.Lerp(motorCycleyRotation, Input.GetAxisRaw("Horizontal"), 0.15f);
                        break;
                    case carType.MotorCycle:
                        float raw = Input.GetAxisRaw("Horizontal") * turnSpeed * Time.fixedDeltaTime * 11;
                        motorCycleyRotation = Mathf.SmoothStep(motorCycleyRotation, raw, 0.1f);
                        print(isleaning);
                        if(!isleaning)
                        {
                            motorCycleLean = motorCycleyRotation;
                        }
                        else
                        {
                            motorCycleLean = Mathf.SmoothStep(motorCycleLean, 0, 0.1f);
                        }
                        
                        //motorCycleTemp = Mathf.Lerp(motorCycleTemp, raw, 0.5f);
                        //print(Mathf.Abs(motorCycleTemp - raw));
                        //if(motorCycle is turning)
                        //motorCycleLean = Mathf.Clamp(Mathf.SmoothDamp(motorCycleLean, (raw) * 30, ref motorCycleLean, 0.2f), -30, 30);
                        //else
                        //motorCycleLean = Mathf.Clamp(Mathf.SmoothDamp(motorCycleLean, 0, ref motorCycleLean, 0.2f), -30, 30);
                        break;
                }
            }
        }

        if (IsUsingSteeringWheel)
        {
            inputHorizontal = SimpleInput.GetAxis(horizontalAxis);

            // Calculate Turning Rotation With Steering Wheel
            float newRot = inputHorizontal * TurnSpeed * Time.deltaTime;

            if (isCarGrounded)
            {

                switch (currentCarType)
                {
                    /*if (isCarGrounded)
                    transform.Rotate(0, newRot, 0, Space.World);*/

                    case carType.Car:
                        //motorCycleyRotation = Mathf.Lerp(motorCycleyRotation, Input.GetAxisRaw("Horizontal"), 0.15f);
                        break;
                    case carType.MotorCycle:
                        float raw = inputHorizontal * turnSpeed * Time.fixedDeltaTime * 11;
                        motorCycleyRotation = Mathf.SmoothStep(motorCycleyRotation, raw, 0.1f);
                        print(isleaning);
                        if (!isleaning)
                        {
                            motorCycleLean = motorCycleyRotation;
                        }
                        else
                        {
                            motorCycleLean = Mathf.SmoothStep(motorCycleLean, 0, 0.1f);
                        }
                        break;
                }
            }

            //SteeringWheelInputs
            if (levelSystem.HasFinished == false)
            {
                yRotation = Mathf.Clamp(yRotation + inputHorizontal * TurnSpeed * Time.deltaTime, -30, 30);
            }
            else
            {
                yRotation = Mathf.Clamp((yRotation + forcedRotation) * TurnSpeed * Time.deltaTime, -30, 30);
                Debug.Log(forcedRotation);
            }
        }

        if (currentCarType == carType.Car)
        {
            transform.eulerAngles = new Vector3(0.0f, yRotation, 0);
            motorBike.transform.localEulerAngles = Vector3.zero;
        }          
        else
        {
            transform.eulerAngles = new Vector3(0.0f, motorCycleyRotation, 0);
            motorBike.transform.localEulerAngles = new Vector3(0.0f, 0, -motorCycleLean * 1.5f);
        }


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
}