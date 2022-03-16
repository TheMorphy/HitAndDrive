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
                print(c.newCarModel.name);
                c.newCarModel.SetActive(true);
            }              
            else
                c.newCarModel.SetActive(false);
        }
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
