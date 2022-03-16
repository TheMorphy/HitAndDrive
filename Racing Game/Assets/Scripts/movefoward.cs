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

        MoveSpeed = (tm.currentlevel / 10) + 5;

        Instantiate(dummy, transform.position, Quaternion.identity);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * MoveSpeed;
    }
}
