using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallUI : MonoBehaviour
{
    [SerializeField] GameObject[] images;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Wall>())
        {
            int randomNumber = Random.Range(1, 7);
            StartCoroutine(WaitToDisable());

            switch (randomNumber)
            {
                case 6:
                    images[0].SetActive(true);
                    break;
                case 5:
                    images[1].SetActive(true);
                    break;
                case 4:
                    images[2].SetActive(true);
                    break;
                case 3:
                    images[3].SetActive(true);
                    break;
                case 2:
                    images[4].SetActive(true);
                    break;
                case 1:
                    images[5].SetActive(true);
                    break;
                default:
                    print("Not a numver");
                    break;
            }
        }
    }

    private IEnumerator WaitToDisable()
    {
        print("WAITING TO DISABLE");
        yield return new WaitForSeconds(0.7f);
        images[0].SetActive(false);
        images[1].SetActive(false);
        images[2].SetActive(false);
        images[3].SetActive(false);
        images[4].SetActive(false);
        images[5].SetActive(false);
        print("DISABLED");
    }
}
