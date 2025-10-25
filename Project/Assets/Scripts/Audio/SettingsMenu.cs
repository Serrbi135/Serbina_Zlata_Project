using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button settingsOpenButton;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private CanvasGroup settingsMenuCG;

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f); 

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        settingsOpenButton.onClick.AddListener(ToggleSettingsMenu);

        settingsMenu.SetActive(false);
    }

    public void SetMasterVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(volume);
        }
        else
        {
            Debug.LogError("AudioManager.Instance is null!");
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }
        else
        {
            Debug.LogError("AudioManager.Instance is null!");
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(volume);
        }
        else
        {
            Debug.LogError("AudioManager.Instance is null!");
        }
    }

    public void ToggleSettingsMenu()
    {
        if(settingsMenu.activeInHierarchy)
        {
            settingsMenuCG.DOFade(0, 0.5f);
        }
        else
        {
            settingsMenuCG.DOFade(1, 0.5f);
        }
        settingsMenu.SetActive(!settingsMenu.activeInHierarchy);
        
    }
}