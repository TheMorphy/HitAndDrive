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

        MoveSpeed = (Mathf.FloorToInt(tm.currentlevel / 10) * 10 / 2);

        if (MoveSpeed >= 25)
        {
            MoveSpeed = 25;
        }

        Instantiate(dummy, transform.position, Quaternion.identity);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * MoveSpeed;

        if (moveSpeed <= 0)
        {
            moveSpeed = 2;
        }
    }
}
