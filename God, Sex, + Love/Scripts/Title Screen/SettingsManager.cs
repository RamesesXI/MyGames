using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    void Start()
    {

        #region Resolution Dropdown Setup
        
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions(); //Need testing, might be redundant
        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        #endregion
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        // Implement Later maybe or just ditch, Needs refreshing dropdown
        //if (!isFullScreen)
        //    Screen.SetResolution(1280, 720, false);
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("MasterVolume", volume);
    }
}
