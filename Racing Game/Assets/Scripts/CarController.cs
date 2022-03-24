using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public enum carType { Car, MotorCycle }

    public carType currentCarType;
    [SerializeField] GameObject motorBike;
    bool isleaning;

    [SerializeField] bool isUsingSteeringWheel, isUsingTouchControl;

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
    public bool IsUsingTouchControl { get => isUsingTouchControl; set => isUsingTouchControl = value; }

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
        if (Mathf.Abs(forcedRotation) < 0.2f)
        {
            forcedRotation = 0;
        }
        // Set Cars Position to Our Sphere
        transform.position = sphereRB.transform.position;

        // Raycast to the ground and get normal to align car with it.
        

        // Rotate Car to align with ground
        

        // Calculate Drag
        sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;

        #region keyboard controls
        if (!IsUsingSteeringWheel && !IsUsingTouchControl)
        {
            // Get Input
            turnInput = Input.GetAxisRaw("Horizontal");

            // Calculate Turning Rotation
            //float newRot = turnInput * TurnSpeed * Time.fixedDeltaTime;

            if (levelSystem.HasFinished == false)
            {
                float raw = Input.GetAxisRaw("Horizontal") * turnSpeed * Time.deltaTime * 11;
                yRotation = Mathf.Lerp(yRotation, raw, 0.1f);
            }
            else
            {
                float raw = Mathf.Clamp(Mathf.Clamp(-forcedRotation / 2, -1, 1) * turnSpeed * Time.deltaTime * 11, -30, 30);
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
                        float raw = Mathf.Clamp(Input.GetAxisRaw("Horizontal") * turnSpeed * Time.deltaTime * 11, -30, 30);
                        motorCycleyRotation = Mathf.SmoothStep(motorCycleyRotation, raw, 0.2f);
                        //print(motorCycleyRotation);
                        if (!isleaning)
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
        #endregion

        #region steering wheel controls and Touch Controls
        if (IsUsingSteeringWheel || IsUsingTouchControl)
        {
            inputHorizontal = SimpleInput.GetAxis(horizontalAxis);

            // Calculate Turning Rotation With Steering Wheel
            //float newRot = inputHorizontal * TurnSpeed * Time.deltaTime;

            if (levelSystem.HasFinished == false)
            {
                float raw = inputHorizontal * turnSpeed * Time.deltaTime * 11;
                yRotation = Mathf.Lerp(yRotation, raw, 0.1f);
            }
            else
            {
                float raw = Mathf.Clamp(Mathf.Clamp(-forcedRotation / 2, -1, 1) * turnSpeed * Time.deltaTime * 11, -30, 30);
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
                        float raw = Mathf.Clamp(inputHorizontal * turnSpeed * Time.deltaTime * 11, -30, 30);
                        motorCycleyRotation = Mathf.SmoothStep(motorCycleyRotation, raw, 0.2f);
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
        }
        #endregion

        if (currentCarType == carType.Car)
        {
            transform.eulerAngles = new Vector3(0.0f, yRotation, 0);
            motorBike.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = new Vector3(0.0f, yRotation, 0);
            motorBike.transform.localEulerAngles = new Vector3(0.0f, 0, -yRotation * 1.5f);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 0.5f, groundLayer);
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.fixedDeltaTime);
        forcedRotation = transform.position.x - TrackManager.instance.transform.position.x;

        if (isCarGrounded)
            sphereRB.AddForce(transform.forward * speed, ForceMode.Acceleration); // Add Movement
        else
            sphereRB.AddForce(transform.up * -200f); // Add Gravity

        carRB.MoveRotation(transform.rotation);
    }

    
}