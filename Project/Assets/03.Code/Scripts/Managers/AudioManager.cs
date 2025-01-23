using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Init();
    }

    [Header("BGM")]
    [SerializeField] private AudioClip[] _bgmClips;
    private AudioSource _bgmPlayer;

    [Header("SFX")]
    [SerializeField] private AudioClip[] _sfxClips;
    public int sfxChannels;
    private AudioSource[] _sfxPlayers;
    private int _channelIndex;

    public enum Bgm { Jegi, Lobby, RGL = 4, Omok, Title }
    public enum Sfx
    {
        ButtonPress, GameLose, GameWin, JegiHit, JegiMiss, Cow, RGL1, RGL2, GetCoin, RGLPD, RGLPEarthQ,
        RGLPGun, Younghee2, Younghee3, Younghee5, OmokTak, Wow, Scream, Aww = 19
    }

    private void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[sfxChannels];
        for (int i = 0; i < sfxChannels; i++)
        {
            _sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[i].playOnAwake = false;
            _sfxPlayers[i].loop = false;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxChannels; i++)
        {
            int loopIndex = (i + _channelIndex) % sfxChannels;
        }
        _sfxPlayers[0].clip = _sfxClips[(int)sfx];
        _sfxPlayers[0].Play();
    }
}
