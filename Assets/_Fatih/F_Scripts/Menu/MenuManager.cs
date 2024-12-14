using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider sensivitySlider;
    [SerializeField] TMP_Text sensText;

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
    }

    public void SetSensivityInMenu()
    {
        PlayerPrefs.SetFloat("Sensivity", sensivitySlider.value);
        sensText.text = (sensivitySlider.value / 30f).ToString("N1");
    }

}
