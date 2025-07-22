using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] UnityEngine.AudioSource unityAudioSource;
    AudioPlayerSource audioSource;
    bool isPause;
    public AudioPlayerSource AudioSource => audioSource;

    public AudioSource UnityAudioSource { get => unityAudioSource; }
    public bool IsPlaying => unityAudioSource.isPlaying;
    public bool IsPause => isPause;
    public bool IsMute => !unityAudioSource.enabled;

    public void Set(AudioPlayerSource audioSource)
    {
        this.audioSource = audioSource;
    }
    public void Play(bool isLoop, float volume)
    {
        unityAudioSource.clip = audioSource.audioClip;
        unityAudioSource.loop = isLoop;
        unityAudioSource.volume = volume;
        unityAudioSource.Play();
        isPause = false;
    }
    public void Stop()
    {
        if (gameObject.activeInHierarchy)
        {
            unityAudioSource.Stop();
            unityAudioSource.gameObject.SetActive(false);
            isPause = false;
        }
    }
    public void Pause()
    {
        if (gameObject.activeInHierarchy && unityAudioSource.isPlaying)
        {
            unityAudioSource.Pause();
            isPause = true;
        }
    }
    public void Resume()
    {
        if (gameObject.activeInHierarchy && !unityAudioSource.isPlaying)
        {
            unityAudioSource.Play();
            isPause = false;
        }
    }
    public void Mute()
    {
        unityAudioSource.enabled = false;
    }
    public void Unmute()
    {
        unityAudioSource.enabled = true;
    }
    public void ChangeSpeed(float speed)
    {
        unityAudioSource.pitch = speed;
    }

    public void ChangeVolume(float volume)
    {
        unityAudioSource.volume = volume;
    }
}
