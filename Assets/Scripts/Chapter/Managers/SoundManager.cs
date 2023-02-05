using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private GameObject soundPrefab;

    private AudioClip[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        AudioClip[] soundsLoad = Resources.LoadAll<AudioClip>("sound");
        AudioClip[] voicesLoad = Resources.LoadAll<AudioClip>("voice");
        sounds = new AudioClip[soundsLoad.Length + voicesLoad.Length];
        Array.Copy(soundsLoad, sounds, soundsLoad.Length);
        Array.Copy(voicesLoad, 0, sounds, soundsLoad.Length, voicesLoad.Length);
    }

    public GameObject Play(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                GameObject sound = Instantiate(soundPrefab);
                sound.GetComponent<SoundDestroy>().Play(sounds[i]);
                return sound;
            }
        }

        Debug.LogError("Invalid Sound: " + name);
        return null;
    }
}
