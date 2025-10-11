using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button settingsOpenButton;
    [SerializeField] private GameObject settingsMenu;

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        settingsOpenButton.onClick.AddListener(SettingsOpen);
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    public void SettingsOpen()
    {
        if(settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
        }
        if(!settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(true);
        }
    }
}