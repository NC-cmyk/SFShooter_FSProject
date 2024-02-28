using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Globalization;
using UnityEngine.Rendering;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private TMP_Text volumeTextvalue;
    [SerializeField] private TMP_Text volumeSFXTextvalue;

    float defaultVolume;
    float defaultSFXVolume;

    public void Start()
    {
        // default volumes
        defaultVolume = 1.0f;
        defaultSFXVolume = 1.0f;

        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            volumeDefaults();
        }
       
    }

    public void SetMusicVolume()
    {
        float volume = soundSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        volumeTextvalue.text = volume.ToString("p0", CultureInfo.InvariantCulture);
    }

    public void SetSFXVolume()
    {
        float SFXvolume = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXvolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXvolume);
        volumeSFXTextvalue.text = SFXvolume.ToString("p0", CultureInfo.InvariantCulture);
    }

    public void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat("musicVolume");
        audioMixer.SetFloat("Music", Mathf.Log10(musicVol) * 20);
        soundSlider.value = musicVol;
        volumeTextvalue.text = musicVol.ToString("p0", CultureInfo.InvariantCulture);

        float sfxVol = PlayerPrefs.GetFloat("SFXVolume");
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVol) * 20);
        SFXSlider.value = sfxVol;
        volumeSFXTextvalue.text = sfxVol.ToString("p0", CultureInfo.InvariantCulture);
    }

    public void volumeDefaults()
    {
        PlayerPrefs.SetFloat("musicVolume", defaultVolume);
        PlayerPrefs.SetFloat("SFXVolume", defaultSFXVolume);
        LoadVolume();
    }
}       