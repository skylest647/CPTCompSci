using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu Panel")]
    [SerializeField] private GameObject pauseMenuPanel;
    
    [Header("Pause Menu Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    
    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button settingsBackButton;
    
    [Header("Volume Controls")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Text volumeText;
    
    private bool isPaused = false;
    private GameManager gameManager;
    private const string VOLUME_KEY = "Volume";

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        SetupButtons();
        LoadVolumeSettings();
        
        // Hide pause menu at start
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        // Toggle pause menu with Escape or P key
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    private void SetupButtons()
    {
        resumeButton.onClick.AddListener(Resume);
        saveButton.onClick.AddListener(SaveGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        quitButton.onClick.AddListener(QuitGame);
        
        settingsBackButton.onClick.AddListener(CloseSettings);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        Time.timeScale = 0f; // Freeze game
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; // Unfreeze game
    }

    private void SaveGame()
    {
        if (gameManager != null)
        {
            gameManager.SaveGame();
            Debug.Log("Game saved from pause menu!");
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }

    private void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Unfreeze before changing scenes
        
        // You can load main menu scene here
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        
        // Or if in same scene, just show main menu
        MainMenuManager mainMenu = FindObjectOfType<MainMenuManager>();
        if (mainMenu != null)
        {
            pauseMenuPanel.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
        
        Debug.Log("Returning to main menu...");
    }

    private void QuitGame()
    {
        Time.timeScale = 1f; // Reset time scale before quitting
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void LoadVolumeSettings()
    {
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.75f);
        volumeSlider.value = volume;
        UpdateVolumeText(volume);
        AudioListener.volume = volume;
    }

    private void OnVolumeChanged(float value)
    {
        UpdateVolumeText(value);
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        AudioListener.volume = value;
    }

    private void UpdateVolumeText(float value)
    {
        volumeText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}