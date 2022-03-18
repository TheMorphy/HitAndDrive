using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{

    [SerializeField] GameObject levelGameobject;
    LevelSystem levelScript;

    private void Start()
    {
        levelScript = levelGameobject.GetComponent<LevelSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //levelScript.Money += 1;
            levelScript.MoneyInOneRound += 1;
            levelScript.UpdateMoney();
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("Collect");
        }
    }
}
