using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Slider sensSlider;
    [SerializeField] TMP_Text sensText;
    [SerializeField] GameObject inGameSettingsPanel;
    [SerializeField] bool pausePanel;

    PlayerController _playerController;

    private void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();

        pausePanel = true;
        SettingsPanel();

        if (PlayerPrefs.HasKey("Sensivity"))
        {
            sensSlider.value = PlayerPrefs.GetFloat("Sensivity");
            SetSensivityInGame();
        }
    }

    public void SettingsPanel()
    {
        pausePanel = !pausePanel;

        if (pausePanel)
        {
            Cursor.lockState = CursorLockMode.None;
            inGameSettingsPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            inGameSettingsPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            
        }
    }

    public void SetSensivityInGame()
    {
        _playerController.SetMouseSensivity(sensSlider.value);
        PlayerPrefs.SetFloat("Sensivity", sensSlider.value);
        sensText.text = (sensSlider.value / 30f).ToString("N1");
    }

    public void FinishTheGame()
    {
        Debug.Log("oyun bitti");
    }
}
