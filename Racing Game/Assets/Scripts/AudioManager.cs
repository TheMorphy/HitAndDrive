using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many Audiomanagers in Scene!");
            return;
        }
        instance = this;
        foreach (Sound s in sounds)
        {
            
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip[Random.Range(0, s.clip.Length - 1)];

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //private void Start()
    //{
    //    Play("Engine");
    //}

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
        }
        s.source.clip = s.clip[Random.Range(0, s.clip.Length - 1)];
        s.source.Play();
    }

    public void Mute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = 0;
    }

    public void Unmute(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
    }

    public void PitchUp(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = pitch;
    }

    public void PitchDown(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = pitch;
    }

    public GameObject PlaceSound(string name, Vector3 position, Transform parent = null, float spatialBlend = 1, float costomVolume = 1, float maxDistance = 30)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        GameObject soundObject = new GameObject();
        soundObject.name = name;
        AudioSource a = soundObject.AddComponent<AudioSource>();
        soundObject.transform.position = position;
        soundObject.transform.parent = parent;
        a.clip = s.clip[Random.Range(0, s.clip.Length)];
        a.spatialBlend = spatialBlend;
        a.volume = s.volume;
        a.loop = s.loop;
        if(!a.loop)
        {
            Destroy(soundObject, a.clip.length);
        }
        a.pitch = s.pitch;
        a.maxDistance = maxDistance;
        a.Play();
        return soundObject;
    }
}
