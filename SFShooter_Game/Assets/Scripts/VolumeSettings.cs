using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundSlider = null;
    [SerializeField] private Slider SFXSlider = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private TMP_Text volumeTextvalue;
    [SerializeField] private TMP_Text volumeSFXTextvalue;
    [SerializeField] private float defaultVolume = 1.0f;
    [SerializeField] private float SFXdefaultVolume = 1.0f;




    public void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
            StartCoroutine(ConfirmationLoadBox());
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
       
    }

    public void SetMusicVolume()
    {
        float volume = soundSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        volumeTextvalue.text = volume.ToString("0.0");
    }

    public void SetSFXVolume()
    {
        float SFXvolume = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXvolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXvolume);
        volumeSFXTextvalue.text = SFXvolume.ToString("0.0");
    }

    public void LoadVolume()
    {
        soundSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        StartCoroutine(ConfirmationLoadBox());
        SetMusicVolume();
        SetSFXVolume();
    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Audio")
        {
            soundSlider.value = defaultVolume;
            SFXSlider.value= SFXdefaultVolume;
            volumeTextvalue.text = defaultVolume.ToString("0.0");
            volumeSFXTextvalue.text = SFXdefaultVolume.ToString("0.0");
            LoadVolume();
        }
    }

    public IEnumerator ConfirmationLoadBox()
    {
       confirmationPrompt.SetActive(true);
       yield return new WaitForSeconds(2);
       confirmationPrompt.SetActive(false);
    }
}       