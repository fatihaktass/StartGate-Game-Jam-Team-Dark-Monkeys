using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Slider sensSlider;
    [SerializeField] TMP_Text sensText;
    [SerializeField] GameObject inGameSettingsPanel;
    [SerializeField] GameObject finalPanel;
    [SerializeField] TMP_Text finalPanelText;
    [SerializeField] bool pausePanel;
    [SerializeField] bool gameFinished;

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
        if (gameFinished)
            return;

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

    public void FinishTheGame(bool didPlayerWin)
    {
        if (!gameFinished)
        {
            StartCoroutine(ResetTheGame());

            Time.timeScale = 0.1f;
            finalPanelText.text = didPlayerWin ? "You won!" : "Busted!";
            finalPanel.SetActive(true);

            gameFinished = true;
        }
    }

    IEnumerator ResetTheGame()
    {
        yield return new WaitForSeconds(.5f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.None;
    }
}
