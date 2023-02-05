using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 BACKGROUND MUSIC IS LOOPED NO MATTER WHAT
 */
public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    private AudioClip[] bgmusic;
    private List<string> queue = new List<string>();

    public static MusicManager instance;

    private bool fading = false;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        bgmusic = Resources.LoadAll<AudioClip>("bgmusic");
    }

    public void Play(string name, string transition, float duration)
    {
        for (int i = 0; i < bgmusic.Length; i++)
        {
            if (bgmusic[i].name == name)
            {
                if (!source.isPlaying)
                {
                    source.clip = bgmusic[i];
                    Resume(transition, duration);
                } else
                {
                    // Is already playing a clip, fade out original and play current
                    queue.Add(name + "," + transition + "," + duration);
                    // coroutine, after it ends queue next
                    
                    if (!fading)
                        End(transition, duration);
                }

                return;
            }
        }

        Debug.LogError("Invalid BGMUSIC: " + name);
    }

    public void Pause(string transition, float duration)
    {
        if (!source.isPlaying)
        {
            Debug.LogError("Cannot Pause AudioSource. Already Paused");
            return;
        }

        if (transition == "FADE")
        {
            StartCoroutine(FadeOut(source, duration, true));
        } else
        {
            source.Pause();
        }
    }

    public void Resume(string transition, float duration)
    {
        if (source.clip == null)
        {
            Debug.LogError("Cannot Resume NULL AudioClip");
            return;
        }

        if (source.isPlaying)
        {
            Debug.LogError("Cannot Resume Source that is Playing already");
            return;
        }

        if (transition == "FADE")
        {
            StartCoroutine(FadeIn(source, duration));
        } else
        {
            source.Play();
        }
    }

    public void End(string transition, float duration)
    {
        if (source.clip == null)
        {
            Debug.LogError("Cannot end NULL AudioClip");
            return;
        }

        if (transition == "FADE")
        {
            StartCoroutine(FadeOut(source, duration, false));
        } else
        {
            source.Stop();
            source.clip = null;
            QueueNext();
        }
    }

    public void QueueNext()
    {
        if (queue.Count == 0) return;
        string[] args = queue[0].Split(',');
        
        Play(args[0], args[1], float.Parse(args[2]));
        queue.RemoveAt(0);
    }

    private IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        if (fading) yield break;
        fading = true;

        audioSource.Play();
        float startVolume = audioSource.volume;
        audioSource.volume = 0;

        for (float i = 0; i <= 200; ++i)
        {
            audioSource.volume = startVolume * (i / 200f);
            yield return new WaitForSeconds(FadeTime / 200f);
        }

        audioSource.volume = startVolume;
        fading = false;
    }
    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime, bool pause)
    {
        if (fading) yield break;

        fading = true;
        float startVolume = audioSource.volume;

        for (float i = 0; i <= 200f; ++i)
        {
            audioSource.volume = startVolume * ((200f - i) / 200f);
            yield return new WaitForSeconds(FadeTime / 200f);
        }

        if (pause)
        {
            audioSource.Pause();
        } else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        
        audioSource.volume = startVolume;
        fading = false;
        QueueNext();
    }
}
