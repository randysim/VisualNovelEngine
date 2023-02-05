using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDestroy : MonoBehaviour
{
    private bool start = false;
    [SerializeField]
    private AudioSource source;

    private void Awake()
    {
        /* GET SOUND EFFECT VOLUME SETTINGS */
        
    }

    private void Update()
    {
        if (start && !source.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
        start = true;
    }
}
