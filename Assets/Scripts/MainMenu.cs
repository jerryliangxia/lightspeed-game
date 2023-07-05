using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool musicToggledOn;
    public bool sfxToggledOn;
    
    public Slider musicSlider;
    public TextMeshProUGUI musicSliderText;
    public GameObject musicSource;
    
    public Slider sfxSlider;
    public TextMeshProUGUI sfxSliderText;

    private void Awake()
    {
        musicSource.GetComponent<AudioSource>().Play();
        musicToggledOn = true;
        sfxToggledOn = true;
        
        // TODO not sure about this
        PlayerPrefs.SetInt("SfxToggledOn", 1);
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        LoadValues();
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MusicVolumeSlider(float volume)
    {
        musicSliderText.text = "Music: " + volume.ToString("0.0");
        var musicVolumeValue = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolumeValue", musicVolumeValue);
        LoadValues();
    }
    
    public void SfxVolumeSlider(float volume)
    {
        sfxSliderText.text = "Sfx: " + volume.ToString("0.0");
        var sfxVolumeValue = sfxSlider.value;
        PlayerPrefs.SetFloat("SfxVolumeValue", sfxVolumeValue);
        LoadValues();
    }

    public void ToggleMusic()
    {
        if (musicToggledOn)
        {
            musicSource.GetComponent<AudioSource>().Pause();
            musicSliderText.text = "Music: Off";
            DisableSlider(musicSlider);
        }
        else
        {
            musicSource.GetComponent<AudioSource>().Play();
            musicSliderText.text = "Music: " + musicSlider.value.ToString("0.0");
            EnableSlider(musicSlider);
        }
        musicToggledOn = !musicToggledOn;
    }
    
    public void ToggleSfx()
    {
        if (sfxToggledOn)
        {
            PlayerPrefs.SetInt("SfxToggledOn", 0);
            sfxSliderText.text = "Sfx: Off";
            DisableSlider(sfxSlider);
        }
        else
        {
            PlayerPrefs.SetInt("SfxToggledOn", 1);
            sfxSliderText.text = "Sfx: " + sfxSlider.value.ToString("0.0");
            EnableSlider(sfxSlider);
        }
        sfxToggledOn = !sfxToggledOn;
    }

    private static void DisableSlider(Selectable slider)
    {
        // Set Slider to disabled
        slider.interactable = false;
        slider.enabled = false;
            
        // Set the opacity to 50%
        var sliderColor = slider.GetComponentInChildren<Image>().color;
        sliderColor.a = 0.5f;
        slider.GetComponentInChildren<Image>().color = sliderColor;
    }

    private static void EnableSlider(Selectable slider)
    {
        // Set Slider to enabled
        slider.interactable = true;
        slider.enabled = true;
            
        // Set the opacity to 100%
        var sliderColor = slider.GetComponentInChildren<Image>().color;
        sliderColor.a = 1f;
        slider.GetComponentInChildren<Image>().color = sliderColor;
    }

    private void LoadValues()
    {
        var musicVolumeValue = PlayerPrefs.GetFloat("MusicVolumeValue");
        musicSlider.value = musicVolumeValue;
        musicSource.GetComponent<AudioSource>().volume = musicVolumeValue;

        var sfxVolumeValue = PlayerPrefs.GetFloat("SfxVolumeValue");
        sfxSlider.value = sfxVolumeValue;
    }
}
