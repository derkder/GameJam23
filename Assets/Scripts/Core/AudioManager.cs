using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioClip> MusicSource;
    public List<AudioClip> SfxSource;
    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;

    private AudioSource _audioSourec;
    private AudioClip _clip;

    public void Start()
    {
        _audioSourec = GetComponent<AudioSource>();
    }

    public void PlayMusic(MusicType mt, bool loop = true)
    {
        _clip = MusicSource[(int)mt];
        _audioSourec.clip = _clip;
        _audioSourec.loop = loop;
        _audioSourec.volume = musicVolume;
        _audioSourec.Play();
    }

    public void StopMusic()
    {
        _audioSourec.Stop();
    }

    public void PlaySFX(SfxType st, bool loop = false)
    {
        _clip = SfxSource[(int)st];
        _audioSourec.clip = _clip;
        _audioSourec.loop = loop;
        _audioSourec.volume = musicVolume;
        _audioSourec.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        _audioSourec.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        _audioSourec.volume = volume;
    }
}
public enum MusicType
{
    TeachLevel = 0,
    MainLevel = 1,
    MainMenu = 2,
}

public enum SfxType
{
    FinishLevel = 0,
    WinLevel = 1,
    TouchGold = 2,
    BallRolling = 3,
    BulletTime = 4,
}
