using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundManager : MonoBehaviour
{
    // In this script we control all the car sounds
    CarController carController;
    float currentSpeed, currentPitch;
    Transform soundPosition;
    [SerializeField]
    string gearShiftSoundName;

    [SerializeField]
    List<CarSound> carSounds = new List<CarSound>();
    // Start is called before the first frame update
    void Start()
    {
        carController = GetComponent<CarController>();
        soundPosition = carController.sphereRB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = carController.speed;
    }

    CarSound getCurrentCarSound()
    {
        CarSound output = null;
        foreach(CarSound c in carSounds)
        {
            if(currentSpeed > c.speedRange.x && currentSpeed < c.speedRange.y)
            {
                output = c;
                currentPitch = pitch(c.speedRange.x, c.speedRange.y, c.pitchRange.x, c.pitchRange.y);
            }
        }
        return output;
    }

    float pitch(float speedStart, float speedEnd, float pitchStart, float pitchEnd)
    {
        float perc = (currentSpeed - speedStart) / speedEnd;
        float dif = (pitchEnd - pitchStart);
        return pitchStart + dif * perc;
    }
}
