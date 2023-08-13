using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip soundClip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("#BGM")]
    AudioSource bgmPlayer;
    public Sound[] bgmSounds;
    public float bgmVolume;

    [Header("#SFX")]
    AudioSource[] sfxPlayers;
    public Sound[] sfxSounds;
    public int channels;
    public float sfxVolume;
    int channelIndex;


    #region singleton
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion singleton

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for (int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBGM(string name)
    {
        if (bgmPlayer == null)
            Init();

        for (int i = 0; i < bgmSounds.Length; ++i)
        {
            if (bgmSounds[i].name == name)
            {
                bgmPlayer.clip = bgmSounds[i].soundClip;
                bgmPlayer.Stop();
                bgmPlayer.Play();
                return;
            }
        }
    }

    public void PlaySFX(string name)
    {
        for(int i = 0; i< sfxSounds.Length; ++i)
        {
            if (sfxSounds[i].name == name)
            {
                for (int j = 0; j < sfxPlayers.Length; ++j)
                {
                    if (!sfxPlayers[j].isPlaying)
                    {
                        sfxPlayers[j].clip = sfxSounds[i].soundClip;
                        sfxPlayers[j].Play();
                        return;
                    }
                }
                Debug.Log("Player is Full");
                return;
            }
        }
        Debug.Log(name + " is not registered");
    }
}
