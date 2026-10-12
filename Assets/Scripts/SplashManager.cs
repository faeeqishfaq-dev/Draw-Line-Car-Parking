using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SplashManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Loading UI")]
    [SerializeField] private Image loadingFiller;
    [SerializeField] private TMP_Text loadingText;

    [Header("Loading Times")]
    [SerializeField] private float firstLoadingTime = 3f;
    [SerializeField] private float secondLoadingTime = 3f;

    [Header("Game Scene")]
    [SerializeField] private string gameSceneName = "Level 1";

    private bool isLoadingGame = false;

    private void Start()
    {
        StartCoroutine(FirstLoading());
    }

    // =========================
    // FIRST LOADING
    // =========================

    private IEnumerator FirstLoading()
    {
        loadingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

        loadingFiller.fillAmount = 0f;

        float timer = 0f;

        while (timer < firstLoadingTime)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(
                timer / firstLoadingTime
            );

            loadingFiller.fillAmount = progress;

            if (loadingText != null)
            {
                loadingText.text =
                    "LOADING... " +
                    Mathf.RoundToInt(progress * 100f) +
                    "%";
            }

            yield return null;
        }

        loadingFiller.fillAmount = 1f;

        if (loadingText != null)
            loadingText.text = "LOADING... 100%";

        yield return new WaitForSeconds(0.2f);

        loadingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // =========================
    // PLAY BUTTON
    // =========================

    public void PlayGame()
    {
        if (isLoadingGame)
            return;

        isLoadingGame = true;
        CheckCpAd.instance.CallAd();
        StartCoroutine(LoadGameScene());
    }

    // =========================
    // SECOND LOADING
    // =========================

    private IEnumerator LoadGameScene()
    {
        mainMenuPanel.SetActive(false);
        loadingPanel.SetActive(true);

        loadingFiller.fillAmount = 0f;

        if (loadingText != null)
            loadingText.text = "LOADING... 0%";

        // Level 1 ko background mein load karna start karo
        CheckCpAd.instance.CallAd();
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(gameSceneName);

        // Scene automatically open nahi hogi
        operation.allowSceneActivation = false;

        float timer = 0f;

        // Loading bar ko exactly secondLoadingTime ke according fill karo
        while (timer < secondLoadingTime)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(
                timer / secondLoadingTime
            );

            loadingFiller.fillAmount = progress;

            if (loadingText != null)
            {
                loadingText.text =
                    "LOADING... " +
                    Mathf.RoundToInt(progress * 100f) +
                    "%";
            }

            yield return null;
        }

        // Ensure bar is fully filled
        loadingFiller.fillAmount = 1f;

        if (loadingText != null)
            loadingText.text = "LOADING... 100%";

        // Agar scene abhi tak background mein ready nahi hui
        // to ready hone ka wait karo
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        // Ab Level 1 open karo
        operation.allowSceneActivation = true;
    }
}