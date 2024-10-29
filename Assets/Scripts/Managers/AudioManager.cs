using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { 
        G18fire, 
        G18r1, 
        G18r2, 
        G18r3, 
        BarretFire,
        BarretR1,
        BarretR2,
        BarretR3,
        BarretR4,
        BarretR5,
        AKr1,
        AKr2,
        AKr3,
        AKr4,
        AKr5,
        AKr6,
        AKr7,
        AKfire1,
        AKfire2,

    }
    void Awake()
    {
        instance = this;
        Init();
    }

    void Start()
    {
        AudioManager.instance.PlayBgm(true);
    }
    void Init()
    {

        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;


        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isplay)
    {
        if (isplay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }
    public void PlaySfx(Sfx sfx)
    {
        bool isPlayed = false;

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (!sfxPlayers[loopIndex].isPlaying)
            {
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
                sfxPlayers[loopIndex].Play();
                channelIndex = (loopIndex + 1) % sfxPlayers.Length;
                isPlayed = true;
                break;
            }
        }

        if (!isPlayed)
        {
            Debug.LogWarning("all SFX channels are in use.");
        }
    }
}
