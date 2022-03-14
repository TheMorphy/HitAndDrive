using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movefoward : MonoBehaviour
{

    int moveSpeed = 5;


    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }
}
