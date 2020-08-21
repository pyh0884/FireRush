using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    AudioSource audioSource;

    IEnumerator currentCorotine;

    //parameter for the gesture audio effect
    float gestureAudioVolumeFix = 0.2f;
    float gestureAudioSpatialBlendFix = 0f;

    //audiosource setting backup
    float volumeBak;
    float spatialBlendBak;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SaveAudioSourceAttribute(); //save the setting of Unity Inspector
        
    }

    public void SaveAudioSourceAttribute()
    {
        //everthing change in the audio system in the code should keep additional save value at here
        volumeBak = audioSource.volume;
        spatialBlendBak = audioSource.volume;

    }

    public void SetGestureAudioFix()
    {
        Debug.Log("set the audio attribute");
        audioSource.volume = gestureAudioVolumeFix;
        audioSource.spatialBlend = gestureAudioSpatialBlendFix;
    }

    public void ResetAudioSourceAttribute()
    {
        //everthing change in the audio system in the code should keep additional save value at here
        audioSource.volume = volumeBak;
        audioSource.spatialBlend = spatialBlendBak;
        StopCoroutine(currentCorotine);
        Debug.Log("reset attribute");
    }

    public void ResetAudioSourceAttributeWithDelay()
    {
        if (currentCorotine != null)
        {
            StopCoroutine(currentCorotine); //stop the other corotine trigger by fingergesture
        }

        currentCorotine = CheckAudioStatus();
        StartCoroutine(currentCorotine);
    }

    IEnumerator CheckAudioStatus()
    {
        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
            
        }
        ResetAudioSourceAttribute();

    }

    //PlayClip at point preventing object destory
    public void SpawnAudio(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, gameObject.transform.position);
    }

    public void SpawnAudio(AudioClip audioClip,float volumeFix)
    {
        AudioSource.PlayClipAtPoint(audioClip, gameObject.transform.position,volumeFix);
    }

    public void SpawnAudio(AudioClip[] audioClips)
    {
        AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Length)],gameObject.transform.position);
    }

    public void SpawnAudio(AudioClip[] audioClips,float volumeFix)
    {
        AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Length)], gameObject.transform.position, volumeFix);
    }

    //play one shot preventing multiple audio playing at same time
    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayAudio(AudioClip[] audioClips)
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }

    public void PlayLoopAudio(AudioClip audioClip)
    {
        audioSource.loop = true;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        //clean setting
        audioSource.loop = false;

        audioSource.Stop();
    }

}
