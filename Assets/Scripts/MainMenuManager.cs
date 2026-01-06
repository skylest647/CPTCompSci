using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneNames;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    
    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    
    [Header("Settings Buttons")]
    [SerializeField] private Button backButton;
    
    [Header("Volume Controls")]
    [SerializeField] private Slider volumeSlider;
    
    [Header("Volume Text")]
    [SerializeField] private Text volumeText;

    private const string VOLUME_KEY = "Volume";

    private void Start()
    {
        SetupButtons();
        LoadVolumeSettings();
        ShowMainMenu();
    }

    private void SetupButtons()
    {
        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(ShowSettings);
        quitButton.onClick.AddListener(QuitGame);
        
        backButton.onClick.AddListener(ShowMainMenu);
        
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void LoadVolumeSettings()
    {
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.75f);
        
        volumeSlider.value = volume;
        UpdateVolumeText(volumeText, volume);
        
        AudioListener.volume = volume;
    }

    private void OnVolumeChanged(float value)
    {
        UpdateVolumeText(volumeText, value);
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        AudioListener.volume = value;
    }

    private void UpdateVolumeText(Text text, float value)
    {
        text.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void StartGame()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        
        if (gameManager != null)
        {
            gameManager.StartNewRun();
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            Debug.Log("New game started!");
        }
        else
        {
            Debug.LogError("GameManager not found in scene!");
        }
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(VOLUME_KEY, 0.75f);
    }
}