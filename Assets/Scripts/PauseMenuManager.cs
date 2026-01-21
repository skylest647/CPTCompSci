using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added for TextMeshPro

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel, settingsPanel, mainMenuPanel, handUIObject;
    [SerializeField] private Button resumeButton, saveButton, settingsButton, mainMenuButton, quitButton, settingsBackButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText; // Changed to TMP
    
    private bool isPaused = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        SetupButtons();
        LoadVolumeSettings();
        
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        // Toggle pause on Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    private void SetupButtons()
    {
        resumeButton.onClick.AddListener(Resume);
        saveButton.onClick.AddListener(() => gameManager?.SaveGame());
        settingsButton.onClick.AddListener(OpenSettings);
        settingsBackButton.onClick.AddListener(CloseSettings);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        quitButton.onClick.AddListener(Application.Quit);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game simulation
    }

    private void Pause()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Freeze game simulation
    }

    private void ReturnToMainMenu()
    {
        Resume();
        if (gameManager != null) gameManager.SetIsInGame(false);

        // Clear visual card objects before leaving
        if (handUIObject != null)
        {
            foreach (Transform child in handUIObject.transform) Destroy(child.gameObject);
            handUIObject.SetActive(false);
        }
        
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // Settings navigation
    private void OpenSettings() { pauseMenuPanel.SetActive(false); settingsPanel.SetActive(true); }
    private void CloseSettings() { settingsPanel.SetActive(false); pauseMenuPanel.SetActive(true); }

    public bool IsPaused() => isPaused;

    private void OnVolumeChanged(float v) 
    { 
        if (volumeText != null) volumeText.text = Mathf.RoundToInt(v * 100) + "%"; 
        PlayerPrefs.SetFloat("Volume", v); 
        AudioListener.volume = v; 
    }

    private void LoadVolumeSettings() 
    { 
        float v = PlayerPrefs.GetFloat("Volume", 0.75f); 
        volumeSlider.value = v; 
        OnVolumeChanged(v); 
    }
}