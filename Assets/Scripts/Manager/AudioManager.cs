using UnityEngine.Audio;
using UnityEngine;
using System;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        [SerializeField]
        private Sound[] _sounds;
        private void Awake()
        {
            instance = this;
            foreach (var sound in _sounds)
            {
                if (sound.source == null)
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
            Sound s = Array.Find(_sounds, x => x.name == name);
            if (s == null) return;
            s.source.Play();
        }

        public void PlayOneShot(string name)
        {
            Sound s = Array.Find(_sounds, x => x.name == name);
            if (s == null) return;
            s.source.PlayOneShot(s.source.clip);
        }
    }

}