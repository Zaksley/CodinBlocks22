using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 


public static class AudioFade {
 
    public static IEnumerator Fade (AudioSource source1, AudioSource source2,float FadeTime) {
        float startVolume = 1f;
 
        while (source1.volume > 0) {
            source1.volume -= startVolume * Time.deltaTime / FadeTime;
            source2.volume += (startVolume * Time.deltaTime / FadeTime);
            yield return null;
        }
    }
 
}

public class AudioHandler : MonoBehaviour
{
    public AudioSource src1;
    public AudioSource src2;
    public AudioClip positive;
    public AudioClip negative;
    public AudioClip up;
    public AudioClip down;

    private IEnumerator coroutine;

    void Start()
    {

        src1.clip = positive;
        src2.clip = negative;

        src1.loop = true;
        src2.loop = true;

        coroutine = AudioFade.Fade(src2,src1, 1f);
        StartCoroutine(coroutine);

        src1.Play();
        src2.Play();

    }

    public void soundup(){
        src1.PlayOneShot(up);
        src2.PlayOneShot(up);
    }
    
    public void sounddown(){
        src1.PlayOneShot(down);
        src2.PlayOneShot(down);
    }

    public void fadepos(){
        StopCoroutine(coroutine);
        coroutine = AudioFade.Fade(src1,src2, 1f);
        StartCoroutine(coroutine);
    }

    public void fadeneg(){
        StopCoroutine(coroutine);
        coroutine = AudioFade.Fade(src2,src1, 1f);
        StartCoroutine(coroutine);
    }
}
