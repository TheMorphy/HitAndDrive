using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSpikes : MonoBehaviour
{
    [SerializeField] GameObject spikes, spawnPoint;

    [SerializeField] float numberOfSpikes;

    float currentSpikes = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WaitAndSummon());
        }
    }

    private IEnumerator WaitAndSummon()
    {
        while (numberOfSpikes > 0)
        {
            numberOfSpikes -= 1;
            yield return new WaitForSeconds(0.5f);
            Instantiate(spikes, spawnPoint.transform.position + new Vector3(-currentSpikes, 0.0f, 0.0f), spawnPoint.transform.rotation);
            currentSpikes += 0.5f;
        }
        
    }

}
