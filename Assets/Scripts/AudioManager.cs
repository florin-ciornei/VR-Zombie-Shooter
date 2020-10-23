using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
       Play("backgroundMusic");
       Play("rain");

       StartCoroutine(ThunderSound());
    }

    private IEnumerator ThunderSound()
    {
        while (true)
        {
            Play("thunder");
            var value = Random.Range(7, 10);

            yield return new WaitForSeconds(value);
        }

    }

    public void Play(string soundName)
    {
        var sound = Array.Find(sounds, s => s.name == soundName);

        try
        {
            sound.source.Play();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("There is no audio with name:" + soundName);
        }
    }

    public void Stop(string soundName)
    {
        var sound = Array.Find(sounds, s => s.name == soundName);

        try
        {
            sound.source.Stop();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("There is no audio with name:" + soundName);
        }
    }
}