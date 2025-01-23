using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI _bgmSoundText;
    [SerializeField] private TextMeshProUGUI _sfxSoundText;

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
        int valueInt = Mathf.RoundToInt(value * 100);
        _bgmSoundText.text = valueInt.ToString();
        _audioMixer.SetFloat("bgm", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("bgm", _bgmSlider.value);
    }

    private void SetSfxVolume(float value)
    {
        int valueInt = Mathf.RoundToInt(value * 100);
        _sfxSoundText.text = valueInt.ToString();
        _audioMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("sfx", _sfxSlider.value);
    }

    public bool CanLoadVolume()
    {
        if (PlayerPrefs.HasKey("bgm") && PlayerPrefs.HasKey("sfx"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadVolume()
    {
        float bgmV = PlayerPrefs.GetFloat("bgm");
        float sfxV = PlayerPrefs.GetFloat("sfx");

        _bgmSlider.value = bgmV;
        _sfxSlider.value = sfxV;

        SetBgmVolume(bgmV);
        SetSfxVolume(sfxV);
    }

    private void OnDestroy()
    {
        _bgmSlider.onValueChanged.RemoveListener(SetBgmVolume);
        _sfxSlider.onValueChanged.RemoveListener(SetSfxVolume);
    }
}
