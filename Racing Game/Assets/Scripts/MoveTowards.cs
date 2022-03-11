using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public GameObject finishLine;
    float speed = 5;
    float posX, posY, posZ;

    Vector3 pos;

    private void Start()
    {
        posX = finishLine.transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
        pos = new Vector3(posX, posY, posZ);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
    }
}
