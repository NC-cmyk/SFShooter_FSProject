using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadPrefabs : MonoBehaviour
{
    [Header("General Settings")]

    [SerializeField] private bool canUse = false;
    [SerializeField] private MainMenuController menuController;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextvalue = null;
    [SerializeField] private Slider soundSlider = null;
    [SerializeField] private TMP_Text volumeSFXTextvalue = null;
    [SerializeField] private Slider SFXSlider = null; 

    [Header("Brightness Settings")]
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private Slider brightnessSilder = null;
    //[SerializeField] private int defaultbrightness = 1;

    [Header("Quality Level Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Sensitivity Settings")]
    [SerializeField] private TMP_Text SensTextValue = null;
    [SerializeField] private Slider SensSilder = null;
    //[SerializeField] private int defaultsens = 5;

    [Header("Invert Y Settings")]
    [SerializeField] private Toggle invertToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("musicVolume");
                volumeTextvalue.text = localVolume.ToString("0.0");
                soundSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }

            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                float SFXLocalVolume = PlayerPrefs.GetFloat("SFXVolume");
                volumeSFXTextvalue.text = SFXLocalVolume.ToString("0.0");
                SFXSlider.value = SFXLocalVolume;
                AudioListener.volume = SFXLocalVolume;
            }

            else
            {
                menuController.ResetButton("Audio");
            }
            
            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
            
            if (PlayerPrefs.HasKey("masterfullscreen"))
            {
                int localFullScreen = PlayerPrefs.GetInt("masterfullscreen");
                
                if (localFullScreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else 
                { 
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }
           
            if (PlayerPrefs.HasKey("masterbrightness"))
            {
                float localBrightness = PlayerPrefs.GetInt("masterbrightness");
                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSilder.value = localBrightness;
            }

            if (PlayerPrefs.HasKey("MasterSens"))
            {
                float localSensitivity = PlayerPrefs.GetInt("MasterSens");
                SensTextValue.text = localSensitivity.ToString("0.0");
                SensSilder.value = localSensitivity;
                menuController.maincontrollerSens = Mathf.RoundToInt(localSensitivity);
            }
           
            if (PlayerPrefs.HasKey("masterInvert"))
            { 
                if (PlayerPrefs.GetInt("masterInvert") == 1)
                {
                    invertToggle.isOn = true;
                }
                else
                {
                    invertToggle.isOn = false;
                }
            }
        }
    }
}
