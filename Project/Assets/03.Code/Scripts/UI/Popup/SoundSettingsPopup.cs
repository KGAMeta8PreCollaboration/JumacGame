using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SoundSettingsPopup : Popup
{
	[SerializeField] private AudioMixer _audioMixer;
	[SerializeField] private Slider _bgmSlider;
	[SerializeField] private Slider _sfxSlider;

	private void Awake()
	{
		_bgmSlider.onValueChanged.AddListener(SetBgmVolume);
		_sfxSlider.onValueChanged.AddListener(SetSfxVolume);
	}

	public void InitVolume()
	{
		SetBgmVolume(_bgmSlider.value);
		SetSfxVolume(_sfxSlider.value);
	}

	private void SetBgmVolume(float value)
	{
		_audioMixer.SetFloat("bgm", Mathf.Log10(value) * 20);
	}

	private void SetSfxVolume(float value)
	{
		_audioMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
	}

	private void OnDestroy()
	{
		_bgmSlider.onValueChanged.RemoveListener(SetBgmVolume);
		_sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
	}
}
