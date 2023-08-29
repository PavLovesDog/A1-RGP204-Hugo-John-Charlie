using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class AudioManager : MonoBehaviour
    {
        GameManager gameManager;

        // References to all audio played in-game
        public AudioSource engineSource;
        public AudioSource accelerationSource;
        public AudioSource crashSource;
        public AudioSource rocketSource;

        public AudioClip engingeSound;
        public AudioClip accelerationSound;
        public AudioClip crashSound;
        public AudioClip rocketSound;


        public float volumeDecreaseAmount = 0.1f;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        // Function to call when another script needs to play a sound
        // takes in the audio play source, an audioclip and volume as float value
        public void PlayAudioOneShot(AudioSource source, AudioClip sound, float volume)
        {
            source.clip = sound;
            source.pitch = Random.Range(0.75f, 1f);
            source.volume = volume;
            source.PlayOneShot(sound);
        }

        public void PlayAudioForDuration(AudioSource source, AudioClip sound, float volume, float time)
        {
            source.clip = sound;
            source.pitch = Random.Range(0.75f, 1f);
            source.volume = volume;
            source.PlayOneShot(sound);
            StartCoroutine(AudioDuration(source, time));
        }

        public void PlayAudio(AudioSource source, AudioClip sound, float volume)
        {
            source.clip = sound;
            source.pitch = Random.Range(0.75f, 1f);
            source.volume = volume;
            source.Play();
        }

        public void StopSound(AudioSource source)
        {
            source.Stop();
        }

        IEnumerator AudioDuration(AudioSource source, float time)
        {
            // hold off untill it stops
            yield return new WaitForSeconds(time);
            source.Stop();
        }
    }
}
