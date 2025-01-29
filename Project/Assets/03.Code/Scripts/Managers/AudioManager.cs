using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Bgm { None = -1, Jegi, Lobby, RGL = 4, Omok, Title, BattleBoss, BattleElite, BattleNormal }
public enum Sfx
{
	ButtonPress, GameLose, GameWin, JegiHit, JegiMiss, Cow, RGL1, RGL2, GetCoin, RGLPD, RGLPEarthQ,
	RGLPGun, Younghee2, Younghee3, Younghee5, OmokTak, Wow, Scream, Aww = 19, BattleHit, BattleLose, BattleReady, BattleStart, BattleWin, BattleHitCritical
}

public class AudioManager : Singleton<AudioManager>
{
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
		Init();
	}
	[Header("MIXER")]
	[SerializeField] private AudioMixer _audioMixer;

	[Header("BGM")]
	[SerializeField] private AudioClip[] _bgmClips;
	private AudioSource _bgmPlayer;

	[Header("SFX")]
	[SerializeField] private AudioClip[] _sfxClips;
	public int sfxChannels;
	private AudioSource[] _sfxPlayers;
	private int _channelIndex;

	private void Init()
	{
		AudioMixerGroup[] group = _audioMixer.FindMatchingGroups("Master");

		GameObject bgmObject = new GameObject("BgmPlayer");
		bgmObject.transform.parent = transform;
		_bgmPlayer = bgmObject.AddComponent<AudioSource>();
		_bgmPlayer.outputAudioMixerGroup = group[1];
		_bgmPlayer.playOnAwake = false;
		_bgmPlayer.volume = 0.7f;
		_bgmPlayer.loop = true;

		GameObject sfxObject = new GameObject("SfxPlayer");
		sfxObject.transform.parent = transform;
		_sfxPlayers = new AudioSource[sfxChannels];
		for (int i = 0; i < sfxChannels; i++)
		{
			_sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
			_sfxPlayers[i].outputAudioMixerGroup = group[2];
			_sfxPlayers[i].playOnAwake = false;
			_sfxPlayers[i].loop = false;
		}
	}

	public void PlaySfx(Sfx sfx)
	{
		for (int i = 0; i < sfxChannels; i++)
		{
			int loopIndex = (i + _channelIndex) % sfxChannels;

			if (_sfxPlayers[loopIndex].isPlaying)
			{
				continue;
			}

			_channelIndex = loopIndex;
			_sfxPlayers[_channelIndex].clip = _sfxClips[(int)sfx];
			_sfxPlayers[_channelIndex].Play();
			break;
		}
	}

	public void PlayBgm(Bgm bgm)
	{
		if (bgm == Bgm.None)
		{
			_bgmPlayer.clip = null;
			return;
		}

		int random = 0;
		if (bgm == Bgm.Lobby)
		{
			random = Random.Range(0, 3);
		}


		_bgmPlayer.clip = _bgmClips[(int)bgm + random];
		_bgmPlayer.Play();
	}

	public float GetClipLength(Sfx sfx)
	{
		return _sfxClips[(int)sfx].length;
	}
}
