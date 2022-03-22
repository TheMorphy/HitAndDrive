using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{

    [SerializeField] GameObject levelGameobject;
    LevelSystem levelScript;
    [SerializeField] ParticleSystem moneyCollectParticle;

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
            ParticleSystem moneyCollect = Instantiate(moneyCollectParticle, transform.position, Quaternion.identity);
            moneyCollect.Play();
            StartCoroutine(DestroyParticle(moneyCollect));
        }
    }

    IEnumerator DestroyParticle(ParticleSystem particle)
    {
        yield return new WaitForSeconds(0.4f);

        Destroy(particle);
    }
}
