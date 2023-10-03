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

    public AudioSource _audioSourceMusic;
    public AudioSource _audioSourceSfx;
    private AudioClip _clip;

    public void PlayMusic(MusicType mt, bool loop = true)
    {
        Debug.Log(mt);
        _clip = MusicSource[(int)mt];
        _audioSourceMusic.clip = _clip;
        _audioSourceMusic.loop = loop;
        _audioSourceMusic.volume = musicVolume;
        _audioSourceMusic.Play();
    }

    public void StopMusic()
    {
        _audioSourceMusic.Stop();
        _audioSourceSfx.Stop();
    }

    public void PlaySFX(SfxType st, bool loop = false)
    {
        _clip = SfxSource[(int)st];
        _audioSourceSfx.clip = _clip;
        _audioSourceSfx.loop = loop;
        _audioSourceSfx.volume = 0.7f * musicVolume;
        _audioSourceSfx.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        _audioSourceMusic.volume = volume;
        _audioSourceSfx.volume = volume;
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
