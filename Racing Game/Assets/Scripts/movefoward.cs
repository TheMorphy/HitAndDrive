using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movefoward : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5;

    [SerializeField] GameObject dummy;

    TrackManager tm;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private void Start()
    {
        tm = FindObjectOfType<TrackManager>();

        if(tm.currentlevel > 9)
        {
            //MoveSpeed = (Mathf.FloorToInt(tm.currentlevel / 10) * 10 / 2);
            MoveSpeed = tm.currentlevel / 2;
            if (MoveSpeed < 8)
            {
                MoveSpeed = 8;
            }
        } else MoveSpeed = 8;


        if (MoveSpeed >= 25)
        {
            MoveSpeed = 25;
        }

        Instantiate(dummy, transform.position, Quaternion.identity);
        tm.car.parent.GetComponent<CarController>().sphereRB.GetComponent<AudioSource>().mute = true;
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * MoveSpeed;

        if (moveSpeed <= 0)
        {
            moveSpeed = 2;
        }

        if (moveSpeed <= 8)
        {
            moveSpeed = 8;
        }
    }
}
