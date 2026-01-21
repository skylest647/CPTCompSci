using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added for TextMeshPro

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] private Button newRunButton, continueButton, settingsButton, quitButton, backButton;

    [Header("Settings UI")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText; // Changed to TMP

    private bool isTransitioning = false;

    private void Start()
    {
        // Add listeners to buttons
        newRunButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(ContinueGame);
        settingsButton.onClick.AddListener(() => ToggleSettings(true));
        backButton.onClick.AddListener(() => ToggleSettings(false));
        quitButton.onClick.AddListener(Application.Quit);
        
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        
        LoadVolumeSettings();
        mainMenuPanel.SetActive(true);
    }

    private void StartNewGame()
    {
        if (isTransitioning) return;
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) 
        { 
            isTransitioning = true; 
            mainMenuPanel.SetActive(false); 
            gm.StartNewRun(); 
            isTransitioning = false; 
        }
    }

    private void ContinueGame()
    {
        // Only load if a save exists and we aren't currently switching scenes
        if (isTransitioning || !SaveSystem.HasSaveData()) return;
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) 
        { 
            isTransitioning = true; 
            mainMenuPanel.SetActive(false); 
            gm.LoadGame(); 
            isTransitioning = false; 
        }
    }

    private void ToggleSettings(bool show)
    {
        settingsPanel.SetActive(show);
        mainMenuPanel.SetActive(!show);
    }

    private void OnVolumeChanged(float v) 
    { 
        volumeText.text = Mathf.RoundToInt(v * 100) + "%"; 
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