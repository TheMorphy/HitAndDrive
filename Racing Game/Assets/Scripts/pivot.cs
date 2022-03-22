using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pivot : MonoBehaviour
{
    [SerializeField]
    GameObject standardCarModel, truckModel, bigTruckModel, feverModel;
    [SerializeField]
    AudioSource motor;
    [SerializeField]
    AudioClip truckSound;
    Vector3 savedCanvasPos;
    [SerializeField]
    Transform canvas;
    [SerializeField]
    Transform carCollider;
    Vector3 v1, v2, v3, v4;
    [SerializeField]
    float smoothingCollider = 0.2f;
    // Start is called before the first frame update
    public void CanvasToPos()
    {
        canvas.localPosition = savedCanvasPos;
    }
    public void ChangeToTruck()
    {
        standardCarModel.SetActive(false);
        truckModel.SetActive(true);
    }

    public void ChangeToBigTruck()
    {
        truckModel.SetActive(false);
        bigTruckModel.SetActive(true);
    }

    public void SaveCanvasPos()
    {
        savedCanvasPos = canvas.localPosition;
       // StartCoroutine(stayatpos());
    }
    
    public void changeToFever()
    {
        feverModel.SetActive(true);
        truckModel.SetActive(false);
        bigTruckModel.SetActive(false);
    }

    public void ChangeToModel(int index)
    {
        foreach(CarChange c in TrackManager.instance.carChanges)
        {
            if (c.index == index)
            {
                //print(c.newCarModel.name);
                c.newCarModel.SetActive(true);
                v1 = c.collider1Center;
                v2 = c.collider1Size;
                v3 = c.collider2Center;
                v4 = c.collider2Size;
            }              
            else
                c.newCarModel.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        carCollider.GetComponent<BoxCollider>().center = Vector3.Lerp(carCollider.GetComponent<BoxCollider>().center, v1, smoothingCollider);
        carCollider.GetComponent<BoxCollider>().size = Vector3.Lerp(carCollider.GetComponent<BoxCollider>().size, v2, smoothingCollider);
        carCollider.transform.GetChild(0).GetComponent<BoxCollider>().center = Vector3.Lerp(carCollider.transform.GetChild(0).GetComponent<BoxCollider>().center, v3, smoothingCollider);
        carCollider.transform.GetChild(0).GetComponent<BoxCollider>().size = Vector3.Lerp(carCollider.transform.GetChild(0).GetComponent<BoxCollider>().size, v4, smoothingCollider);
    }

    public void changeFromFever()
    {

    }

    IEnumerator stayatpos()
    {
        while(false == false)
        {
            yield return null;
            print("dasdadgaudiwadGU");
            CanvasToPos();

        }
    }
}
