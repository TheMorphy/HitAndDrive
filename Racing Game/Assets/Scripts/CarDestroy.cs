using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroy : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject collisionParticle;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] ParticleSystem levelUpParticle;
    [SerializeField] GameObject emoji;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject ls;

    bool zooming;
    [SerializeField] GameObject speedParticle;
    [SerializeField] AudioSource engineSound;
    [SerializeField] GameObject car;

    LevelSystem levelSystem;

    private void Start()
    {
        if(levelSystem != null)
        levelSystem = ls.GetComponent<LevelSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 14)
        {
            if(TrackManager.instance.fever)
            {
                collision.rigidbody.AddForce(transform.forward * TrackManager.instance.carCrashPower * 1.3f, ForceMode.Impulse);
                collision.rigidbody.AddTorque(Random.Range(-1f, 1f) * 10, Random.Range(-1f, 1f) * 10, Random.Range(-1f, 1f) * 10, ForceMode.Force);
            }
            else
            {
                collision.rigidbody.AddForce(transform.forward * TrackManager.instance.carCrashPower, ForceMode.Impulse);
                collision.rigidbody.AddTorque(Random.Range(-1f, 1f) * 5, Random.Range(-1f, 1f) * 5, Random.Range(-1f, 1f) * 5, ForceMode.Force);

            }

        }
        if (collision.gameObject.layer == 9)
        {
            //Instantiate(collisionParticle, collision.transform.position + new Vector3(0, 0.1f, 0), transform.rotation * Quaternion.Euler(-90f, 0, 0f));
            levelUpParticle.Play();
        }

        if (collision.gameObject.layer == 11)
        {
            if(TrackManager.instance.fever)
            {
                Instantiate(explosionParticle, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
                
                return;
            }
            int levelsToLose = 5;
            if(TrackManager.instance.currentlevel - levelsToLose <= 0)
            {
                TrackManager.instance.PlayerDie(transform.position);
            }
            else
            {
                Instantiate(explosionParticle, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
            }
            TrackManager.instance.changeLevel(-levelsToLose, "", true);

                    

        }

        if (collision.gameObject.layer == 12)
        {
            Instantiate(collisionParticle, collision.GetContact(0).point, Quaternion.identity);
            Instantiate(emoji, collision.GetContact(0).point, transform.rotation * Quaternion.Euler(40f, 0, 0f));
        }

        if (collision.gameObject.layer == 13)
        {
            zooming = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 14 && collision.rigidbody.velocity.magnitude < 0.5f)
        {
            Destroy(collision.gameObject);
            Instantiate(explosionParticle, collision.transform.position, Quaternion.identity);

        }
    }

    private void Update()
    {
        
        if (zooming)
        {
            
            StartCoroutine(LerpFunction(64, 0.3f));
            zooming = false;
        }
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        
        StartCoroutine(LerpSound(2f, 0.3f));
        float time = 0;
        float startValue = mainCamera.fieldOfView;
        speedParticle.SetActive(true);
        while (time < duration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        mainCamera.fieldOfView = endValue;
        
        yield return new WaitForSeconds(2.5f);

        time = 0;
        speedParticle.SetActive(false);
        while (time < duration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(endValue, startValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        mainCamera.fieldOfView = startValue;
    }

    IEnumerator LerpSound(float endValue, float duration)
    {
        
        float time = 0;
        float startValue = engineSound.pitch;
        while (time < duration)
        {
            
            engineSound.pitch = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        engineSound.pitch = endValue;

        yield return new WaitForSeconds(2.5f);

        time = 0;
        while (time < duration)
        {
            engineSound.pitch = Mathf.Lerp(endValue, startValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        engineSound.pitch = startValue;
    }
}
