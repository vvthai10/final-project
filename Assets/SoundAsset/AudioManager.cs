using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private void Awake()
    {
        foreach (var sound in sounds)
        {
            if(sound.source == null)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
            }
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            //set to 3d sound
            sound.source.spatialBlend = 1f;
            //sound.source.Play();
        }
    }

    private void Start()
    {
       
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, x => x.name == name);
        if (s == null) return;
        s.source.Play();
    }
}
