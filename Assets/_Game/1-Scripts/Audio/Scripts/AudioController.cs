using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudioController : Singleton<AudioController>, IObjectPoolUser<AudioPlayer>
{
    [SerializeField] int capacityAudioMultiPlaying = 4;
    [SerializeField] List<AudioPlayerSource> lstAudioPlayerSource = new List<AudioPlayerSource>();
    [SerializeField] AudioPlayer audioPlayerPrefab;
    Dictionary<AudioName, AudioPlayer> audioPlayerSinglePlaying = new Dictionary<AudioName, AudioPlayer>();
    Dictionary<AudioName, List<AudioPlayer>> audioPlayerMultiPlaying = new Dictionary<AudioName, List<AudioPlayer>>();
    Dictionary<AudioName, AudioSettings> audioPlayingSettings = new Dictionary<AudioName, AudioSettings>();
    Pooler<AudioPlayer> audioPlayerPool;
    float speed = 1;
    bool isMuteSFX = false;
    bool isMuteMusic = false;
    public event Action<bool> onChangeSFXMuteState;
    public event Action<bool> onChangeMusicMuteState;

    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            foreach (var audioPlayer in audioPlayerSinglePlaying.Values)
            {
                audioPlayer.ChangeSpeed(value);
            }

            foreach (var lstAudioPlayer in audioPlayerMultiPlaying.Values)
            {
                foreach (var audioPlayer in lstAudioPlayer)
                {
                    audioPlayer.ChangeSpeed(value);
                }
            }
        }
    }

    private float soundVolume => DBController.Instance.USER_SETTINGS.sound;
    private float musicVolume => DBController.Instance.USER_SETTINGS.music;
    
    public List<AudioPlayerSource> LstAudioPlayerSource
    {
        get => lstAudioPlayerSource;
    }

    public void GetFromPool(AudioPlayer audioPlayer)
    {
        audioPlayer.gameObject.SetActive(true);
    }

    public void ReturnToPool(AudioPlayer audioPlayer)
    {
        if (!audioPlayer.UnityAudioSource.enabled)
        {
            audioPlayer.UnityAudioSource.enabled = true;
        }

        RemoveFromPlayingList(audioPlayer);
        audioPlayer.gameObject.SetActive(false);
    }

    protected override void CustomAwake()
    {
        Initialized();
        GameEvent.onChangeSkin += PlayBG;
        GameEvent.onChangeVolume += OnChangeVolumeByType;
    }

    private void OnDestroy()
    {
        GameEvent.onChangeVolume -= OnChangeVolumeByType;
        GameEvent.onChangeSkin -= PlayBG;
    }

    private void Start()
    {
        // CheckMusic();
        // CheckSFX();
        OnChangeVolumeByType(TypeVolume.Music, musicVolume);
        OnChangeVolumeByType(TypeVolume.Sound, soundVolume);
        Play(AudioName.Music_BG_Loading,true);
    }

    void Initialized()
    {
        audioPlayerPool = new Pooler<AudioPlayer>(audioPlayerPrefab
            , transform, this);
    }

    private void OnChangeVolumeByType(TypeVolume typeVolume, float value)
    {
        switch (typeVolume)
        {
            case TypeVolume.Music:
                ChangeVolumeAction(TypeAudio.Music, value);
                break;
            case TypeVolume.Sound:
                ChangeVolumeAction(TypeAudio.SFX, value);
                break;
        }
    }

    public void PlayBG(ShopCategory typeSkin, SkinType typeRegion)
    {
        if (typeSkin != ShopCategory.BACKGROUND) return;
        StopAll(TypeAudio.Music);
        switch (typeRegion)
        {
            case SkinType.CLASSIC:
                Play(AudioName.Music_BG_Classic, true);
                break;
            case SkinType.CLOWN:
                Play(AudioName.Music_BG_Clown, true);
                break;
            case SkinType.DRAGON:
                Play(AudioName.Music_BG_Dragon, true);
                break;
            case SkinType.SEA:
                Play(AudioName.Music_BG_Sea, true);
                break;
        }
    }

    public void PlayBG(AudioName audioName)
    {
        StopAll(TypeAudio.Music);
        Play(audioName, true);
    }

    public void Play(AudioPlayerSource audioPlayerSource, bool isLoop, float volume = 1)
    {
        if (audioPlayerSource.audioClip == null)
        {
            return;
        }

        if (audioPlayerSource.typeAudioInstance == TypeAudioInstance.Single)
        {
            if (!audioPlayerSinglePlaying.ContainsKey(audioPlayerSource.audioName))
            {
                AudioPlayer audioPlayer = audioPlayerPool.Pool.Get();
                audioPlayerSinglePlaying.Add(audioPlayerSource.audioName, audioPlayer);
                audioPlayingSettings.Add(audioPlayerSource.audioName, new AudioSettings()
                {
                    loop = isLoop,
                    volume = volume,
                });
                audioPlayer.Set(audioPlayerSource);
                audioPlayer.ChangeSpeed(1);
                if (!audioPlayerSource.isSpeedConstant)
                {
                    audioPlayer.ChangeSpeed(speed);
                }

                audioPlayer.Play(isLoop, volume);
                SetPlayWhenMute(audioPlayerSource.typeAudio, audioPlayer);
            }
        }
        else
        {
            AudioPlayer audioPlayer = audioPlayerPool.Pool.Get();
            if (!audioPlayerMultiPlaying.ContainsKey(audioPlayerSource.audioName))
            {
                audioPlayerMultiPlaying.Add(audioPlayerSource.audioName,
                    new List<AudioPlayer>(capacityAudioMultiPlaying));
                audioPlayingSettings.Add(audioPlayerSource.audioName, new AudioSettings()
                {
                    loop = isLoop,
                    volume = volume,
                });
            }

            audioPlayerMultiPlaying[audioPlayerSource.audioName].Add(audioPlayer);
            audioPlayer.Set(audioPlayerSource);
            audioPlayer.ChangeSpeed(1);
            if (!audioPlayerSource.isSpeedConstant)
            {
                audioPlayer.ChangeSpeed(speed);
            }

            audioPlayer.Play(isLoop, volume);
            SetPlayWhenMute(audioPlayerSource.typeAudio, audioPlayer);
        }
    }

    void SetPlayWhenMute(TypeAudio typeAudio, AudioPlayer audioPlayer)
    {
        if ((typeAudio == TypeAudio.SFX && isMuteSFX) ||
            (typeAudio == TypeAudio.Music && isMuteMusic))
        {
            audioPlayer.Mute();
        }
    }

    public void Play(AudioName audioName, bool isLoop = false)
    {
        int _audioSourceIndex =
            lstAudioPlayerSource.FindIndex((audioSource) => audioName.Equals(audioSource.audioName));
        if (_audioSourceIndex == -1)
        {
            Debug.LogError("Audio Not Found");
            return;
        }

        var volume = lstAudioPlayerSource[_audioSourceIndex].typeAudio == TypeAudio.Music
            ? musicVolume
            : soundVolume;
        Play(lstAudioPlayerSource[_audioSourceIndex], isLoop, volume);
    }

    public void SetVolume(AudioName audioName, float volume)
    {
        bool isChangeVolumeSuccess = false;
        if (audioPlayerSinglePlaying.ContainsKey(audioName))
        {
            audioPlayerSinglePlaying[audioName].UnityAudioSource.volume = volume;
            isChangeVolumeSuccess = true;
        }
        else if (audioPlayerMultiPlaying.ContainsKey(audioName))
        {
            foreach (AudioPlayer audioPlayer in audioPlayerMultiPlaying[audioName])
            {
                audioPlayer.UnityAudioSource.volume = volume;
            }

            isChangeVolumeSuccess = true;
        }

        if (isChangeVolumeSuccess)
        {
            var audioSetting = audioPlayingSettings[audioName];
            audioSetting.volume = volume;
            audioPlayingSettings[audioName] = audioSetting;
        }
    }

    public void Stop(AudioName audioName)
    {
        if (audioPlayerSinglePlaying.ContainsKey(audioName))
        {
            audioPlayerSinglePlaying[audioName].Stop();
        }
        else if (audioPlayerMultiPlaying.ContainsKey(audioName))
        {
            for (int i = audioPlayerMultiPlaying[audioName].Count - 1; i >= 0; i--)
            {
                audioPlayerMultiPlaying[audioName][i].Stop();
            }
        }
    }

    public void Pause(AudioName audioName)
    {
        if (audioPlayerSinglePlaying.ContainsKey(audioName))
        {
            audioPlayerSinglePlaying[audioName].Pause();
        }
        else if (audioPlayerMultiPlaying.ContainsKey(audioName))
        {
            foreach (AudioPlayer audioPlayer in audioPlayerMultiPlaying[audioName])
            {
                audioPlayer.Pause();
            }
        }
    }

    public void Resume(AudioName audioName)
    {
        if (audioPlayerSinglePlaying.ContainsKey(audioName))
        {
            audioPlayerSinglePlaying[audioName].Resume();
        }
        else if (audioPlayerMultiPlaying.ContainsKey(audioName))
        {
            foreach (AudioPlayer audioPlayer in audioPlayerMultiPlaying[audioName])
            {
                audioPlayer.Resume();
            }
        }
    }

    public bool TryGetAudioInfo(AudioName audioName, out AudioSettings audioSettings)
    {
        return audioPlayingSettings.TryGetValue(audioName, out audioSettings);
    }

    #region Helper

    IEnumerable<AudioName> AudioNameWithType(TypeAudio audioType)
    {
        foreach (AudioPlayerSource audioSource in lstAudioPlayerSource)
        {
            if (audioSource.typeAudio == audioType)
            {
                yield return audioSource.audioName;
            }
        }
    }

    void RemoveFromPlayingList(AudioPlayer audioPlayer)
    {
        switch (audioPlayer.AudioSource.typeAudioInstance)
        {
            case TypeAudioInstance.Single:
                if (audioPlayerSinglePlaying.ContainsKey(audioPlayer.AudioSource.audioName))
                {
                    audioPlayerSinglePlaying.Remove(audioPlayer.AudioSource.audioName);
                    audioPlayingSettings.Remove(audioPlayer.AudioSource.audioName);
                }

                break;
            case TypeAudioInstance.Multi:
                if (audioPlayerMultiPlaying.ContainsKey(audioPlayer.AudioSource.audioName))
                {
                    if (audioPlayerMultiPlaying[audioPlayer.AudioSource.audioName].Remove(audioPlayer))
                    {
                        if (audioPlayerMultiPlaying[audioPlayer.AudioSource.audioName].Count == 0)
                        {
                            audioPlayerMultiPlaying.Remove(audioPlayer.AudioSource.audioName);
                            audioPlayingSettings.Remove(audioPlayer.AudioSource.audioName);
                        }
                    }
                }

                break;
        }
    }

    #endregion

    #region Mute

    void MuteAction(TypeAudio audioType)
    {
        var lstAudioNameWithType = AudioNameWithType(audioType);
        foreach (AudioName audioName in lstAudioNameWithType)
        {
            if (audioPlayerSinglePlaying.ContainsKey(audioName))
            {
                audioPlayerSinglePlaying[audioName].Mute();
            }

            if (audioPlayerMultiPlaying.ContainsKey(audioName))
            {
                foreach (AudioPlayer audioPlayer in audioPlayerMultiPlaying[audioName])
                {
                    audioPlayer.Mute();
                }
            }
        }
    }

    void ChangeVolumeAction(TypeAudio audioType, float value)
    {
        var lstAudioNameWithType = AudioNameWithType(audioType);
        foreach (AudioName audioName in lstAudioNameWithType)
        {
            if (audioPlayerSinglePlaying.ContainsKey(audioName))
            {
                audioPlayerSinglePlaying[audioName].ChangeVolume(value);
            }

            if (audioPlayerMultiPlaying.ContainsKey(audioName))
            {
                foreach (AudioPlayer audioPlayer in audioPlayerMultiPlaying[audioName])
                {
                    audioPlayer.ChangeVolume(value);
                }
            }
        }
    }

    public void TemporarilyMuteMusic()
    {
        MuteAction(TypeAudio.Music);
    }

    public void MuteMusic(Action onMuted = default)
    {
        isMuteMusic = true;
        onChangeMusicMuteState?.Invoke(isMuteMusic);
        MuteAction(TypeAudio.Music);
        onMuted?.Invoke();
    }

    public void TemporarilyMuteSFX()
    {
        MuteAction(TypeAudio.SFX);
    }

    public void MuteSFX(Action onMuted = default)
    {
        isMuteSFX = true;
        onChangeSFXMuteState?.Invoke(isMuteSFX);
        MuteAction(TypeAudio.SFX);
        onMuted?.Invoke();
    }

    public void ChangeSpeed(float speed)
    {
        foreach (var audioPlayer in audioPlayerSinglePlaying.Values)
        {
            if (!audioPlayer.AudioSource.isSpeedConstant)
            {
                audioPlayer.ChangeSpeed(speed);
            }
        }

        foreach (var lstAudioPlayer in audioPlayerMultiPlaying.Values)
        {
            foreach (var audioPlayer in lstAudioPlayer)
            {
                if (!audioPlayer.AudioSource.isSpeedConstant)
                {
                    audioPlayer.ChangeSpeed(speed);
                }
            }
        }
    }

    #endregion

    #region Unmute

    void UnmuteAction(TypeAudio audioType)
    {
        foreach (AudioName audioName in AudioNameWithType(audioType))
        {
            if (audioPlayerSinglePlaying.ContainsKey(audioName))
            {
                audioPlayerSinglePlaying[audioName].Unmute();
            }

            if (audioPlayerMultiPlaying.ContainsKey(audioName))
            {
                foreach (AudioPlayer audioPlayer in audioPlayerMultiPlaying[audioName])
                {
                    audioPlayer.Unmute();
                }
            }
        }
    }

    public void UnmuteMusic(Action onUnmuted = default)
    {
        isMuteMusic = false;
        onChangeMusicMuteState?.Invoke(isMuteMusic);
        UnmuteAction(TypeAudio.Music);
        onUnmuted?.Invoke();
    }

    public void UnmuteSFX(Action onUnmuted = default)
    {
        isMuteSFX = false;
        onChangeSFXMuteState?.Invoke(isMuteSFX);
        UnmuteAction(TypeAudio.SFX);
        onUnmuted?.Invoke();
    }

    #endregion

    public void StopAll(TypeAudio audioType)
    {
        foreach (AudioName audioName in AudioNameWithType(audioType))
        {
            if (audioPlayerSinglePlaying.ContainsKey(audioName))
            {
                audioPlayerSinglePlaying[audioName].Stop();
            }

            if (audioPlayerMultiPlaying.ContainsKey(audioName))
            {
                for (int i = audioPlayerMultiPlaying[audioName].Count - 1; i >= 0; i--)
                {
                    audioPlayerMultiPlaying[audioName][i].Stop();
                }
            }
        }
    }
}