using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider sensivitySlider;
    [SerializeField] TMP_Text sensText;
    [SerializeField] Slider musicSlider;
    [SerializeField] TMP_Text musicVolumeText;
    [SerializeField] AudioSource musicSource;

    void Start()
    {
        if (PlayerPrefs.HasKey("Sensivity"))
        {
            sensivitySlider.value = PlayerPrefs.GetFloat("Sensivity");
        }
        else
        {
            SetSensivityInMenu();
            sensivitySlider.value = PlayerPrefs.GetFloat("Sensivity");
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("Music");
            musicSource.volume = PlayerPrefs.GetFloat("Music");
        }
        else
        {
            SetMusicVolumeInMenu();
            musicSlider.value = PlayerPrefs.GetFloat("Music");
        }
    }

    public void SetSensivityInMenu()
    {
        PlayerPrefs.SetFloat("Sensivity", sensivitySlider.value);
        sensText.text = (sensivitySlider.value / 30f).ToString("N1");
    }

    public void SetMusicVolumeInMenu()
    {
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        musicVolumeText.text = (musicSlider.value * 100).ToString();
        musicSource.volume = musicSlider.value;
    }

}
