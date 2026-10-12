using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Levels")]
    [SerializeField] private GameObject[] levels;

    [Header("Panels")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject levelFailPanel;

    [Header("Optional")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Button nextLevelButton;

    [Header("Main Menu")]
    [SerializeField] private string mainMenuSceneName = "Splash";

    private int currentLevelIndex;

    // Retry ke baad same level open rakhne ke liye.
    private static int levelToOpen = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OpenLevel(levelToOpen);
    }

    private void OpenLevel(int index)
    {
        currentLevelIndex = Mathf.Clamp(index, 0, levels.Length - 1);

        // Pehle sab levels OFF.
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(false);
        }

        // Current level ON.
        levels[currentLevelIndex].SetActive(true);

        levelCompletePanel.SetActive(false);
        levelFailPanel.SetActive(false);

        if (levelText != null)
        {
            levelText.text = "LEVEL " + (currentLevelIndex + 1);
        }

        // Level 5 par Next Level hide.
        if (nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(
                currentLevelIndex < levels.Length - 1
            );
        }

        // Game manager ko naye level ke routes batao.
        Game.Instance.PrepareLevel(levels[currentLevelIndex]);
    }

    public void ShowLevelComplete()
    {
        levelFailPanel.SetActive(false);
        levelCompletePanel.SetActive(true);
    }

    public void ShowLevelFail()
    {
        levelCompletePanel.SetActive(false);
        levelFailPanel.SetActive(true);
    }

    public void NextLevel()
    {
        CheckCpAd.instance.CallAd();
        if (currentLevelIndex >= levels.Length - 1)
            return;

        currentLevelIndex++;

        levelToOpen = currentLevelIndex;

        OpenLevel(currentLevelIndex);
    }

    public void RetryLevel()
    {
        // Current level number remember karo.
        levelToOpen = currentLevelIndex;

        // Same Unity scene reload hogi.

        CheckCpAd.instance.CallAd();
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void MainMenu()
    {
        levelToOpen = 0;
        CheckCpAd.instance.CallAd();
        SceneManager.LoadScene("Splash");
    }
}