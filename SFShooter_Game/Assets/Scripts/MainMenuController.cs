using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Levels To Load")]

    public string _newGameLevel;
    public string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Gameplay Settings")]

    [SerializeField] private TMP_Text SensTextValue = null;
    [SerializeField] private Slider SensSilder = null;
    [SerializeField] private int defaultsens = 5;
    public int maincontrollerSens = 5;

    [Header("Toggle Settings")]

    [SerializeField] private Toggle invertToggle = null;

    [Header("Graphic Settings")]

    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private Slider brightnessSilder = null;
    [SerializeField] private int defaultbrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isfullscreen;
    private float _brightnessLevel;



    [Header("Resolution Dropdowns")]

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        // if the player went back to the main menu from the game
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
        }

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGameDialogYes()
    {
        // just to make sure
        PlayerPrefs.DeleteKey("SavedLevel");

        StartCoroutine(loadScene(_newGameLevel));
    }

    public void LoadGameDialogYes()
    {
        bool hasSavedLevel = PlayerPrefs.HasKey("SavedLevel");
        if (hasSavedLevel)
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            StartCoroutine(loadScene(levelToLoad));
        }
        else if(!hasSavedLevel)
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetSensititvity(float sensitity)
    {
        maincontrollerSens = Mathf.RoundToInt(sensitity);
        SensTextValue.text = sensitity.ToString("0");
    }
    public void GameplayApply()
    {
        if (invertToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvert", 1);
            //invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvert", 0);
            //Dont invert Y
        }

        PlayerPrefs.SetFloat("MasterSens", maincontrollerSens);

    }
    public void ResetButton(string MenuType)
    {
        if (MenuType == "Gameplay")
        {
            SensTextValue.text = defaultsens.ToString("0");
            SensSilder.value = defaultsens;
            maincontrollerSens = defaultsens;
            invertToggle.isOn = false;
            GameplayApply();
        }

        if (MenuType == "Graphics")
        {
            //Reset brightness value
            brightnessSilder.value = defaultbrightness;
            brightnessTextValue.text = defaultbrightness.ToString("0.0");


            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }
    public void SetFullscreen(bool isfullscreen)
    {
        _isfullscreen = isfullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterbrightness", _brightnessLevel);
        //Change brightness with int post process 
        PlayerPrefs.SetFloat("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("masterfullscreen", (_isfullscreen ? 1 : 0));
        Screen.fullScreen = _isfullscreen;

    }

    IEnumerator loadScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameManager.instance.playerScript.respawn();
    }
}

   