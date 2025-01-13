using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public MobSpawnController spawnController;
    public CanvasGroup blackScreen;
    public GameObject resultScreen;
    public GameObject pauseMenu;

    [Header("Calendar")]
    public TMP_Text dayTxt;
    public TMP_Text timeTxt;
    public GameObject endBtn;

    [Header("Stats")]
    public TMP_Text cashTxt;
    public TMP_Text arnibalTxt;
    public Slider arnibalSlider;
    public TMP_Text soyaTxt;
    public Slider soyaSlider;
    public TMP_Text sagoTxt;
    public Slider sagoSlider;

    [Header("Result Screen")]
    public CanvasGroup nando;
    public GameObject sold;
    public TMP_Text soldTxt;
    public GameObject earned;
    public TMP_Text earnedTxt;
    public GameObject total;
    public TMP_Text totalTxt;
    public GameObject again;
    public GameObject menu;

    int arnibalStock = 75;
    int soyaStock = 75;
    int sagoStock = 75;

    float timer = 0;
    int hour = 5;
    bool dayEnded = false;
    int totalCash;
    int currentCash;
    int served;
    private void Start()
    {
        dayTxt.SetText($"DAY: {(PlayerPrefs.HasKey("Day") ? PlayerPrefs.GetInt("Day") : 1)}");
        cashTxt.SetText((totalCash = PlayerPrefs.HasKey("Cash") ? PlayerPrefs.GetInt("Cash") : 0).ToString());
        timeTxt.SetText($"TIME: {hour}:00");
        arnibalTxt.SetText($"{arnibalStock}/75");
        arnibalSlider.value = arnibalStock/75f;
        soyaTxt.SetText($"{soyaStock}/75");
        soyaSlider.value = soyaStock/75f;
        sagoTxt.SetText($"{sagoStock}/75");
        sagoSlider.value = sagoStock/75f;

        AudioHandler.instance?.PlaySFX("Start");
    }

    private void Update()
    {
        if (!dayEnded)
        {
            if (hour < 20)
                timer += Time.deltaTime;
            else
                EndDay();

            if (timer >= 15f)
            {
                hour++;
                hour = Mathf.Clamp(hour, 5, 20);
                timeTxt.SetText($"TIME: {hour}:00");
                timer = 0;
            }

            if (hour >= 12)
                endBtn.SetActive(true);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    public void UpdateCash()
    {
        served++;
        currentCash += 15;
        cashTxt.SetText((totalCash+currentCash).ToString());
        AudioHandler.instance?.PlaySFX("Coin");
    }

    public void EndDay()
    {
        spawnController.EndFunction();
        AudioHandler.instance?.PlaySFX("End");

        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            blackScreen.interactable = true;
            blackScreen.blocksRaycasts = true;

            while (blackScreen.alpha < 1)
            {
                blackScreen.alpha += Time.deltaTime / 0.6f;
                yield return null;
            }
            blackScreen.alpha = 1;

            yield return new WaitForSeconds(0.8f);

            resultScreen.SetActive(true);

            while(nando.alpha < 1)
            {
                nando.alpha += Time.deltaTime / 2f;
                yield return null;
            }
            nando.alpha = 1;

            sold.SetActive(true);
            float currentSold = 0;
            var soldRate = Mathf.Abs(served - currentSold) / 0.8f;
            while(currentSold != served)
            {
                currentSold = Mathf.MoveTowards(currentSold, served, soldRate * Time.deltaTime);
                soldTxt.text = ((int)currentSold).ToString();
                yield return null;
            }
            yield return new WaitForSeconds(0.6f);

            earned.SetActive(true);
            float currentEarned = 0;
            var earnRate = Mathf.Abs(currentCash - currentEarned) / 0.8f;
            while(currentEarned != currentCash)
            {
                currentEarned = Mathf.MoveTowards(currentEarned, currentCash, earnRate * Time.deltaTime);
                earnedTxt.text = ((int)currentEarned).ToString();
                yield return null;
            }
            yield return new WaitForSeconds(0.6f);

            total.SetActive(true);
            float currentTotal = totalCash+=currentCash;
            var totalRate = currentCash == 0 ? (Mathf.Abs(currentTotal - currentCash) / 0.8f) : 0;
            while (currentTotal != (totalCash))
            {   
                currentTotal = Mathf.MoveTowards(currentTotal, totalCash, totalRate * Time.deltaTime);
                totalTxt.text = ((int)currentTotal).ToString();
                yield return null;
            }
            yield return new WaitForSeconds(0.6f);

            again.SetActive(true);
            menu.SetActive(true);

            PlayerPrefs.SetInt("Cash", totalCash);
            int d = PlayerPrefs.GetInt("Day");
            PlayerPrefs.SetInt("Day", d++);

            yield return null;
        }
    }

    public void OpenSettings() => UnityEngine.SceneManagement.SceneManager.LoadScene("Settings", UnityEngine.SceneManagement.LoadSceneMode.Additive);

    public void ExitGame()
    {
        Time.timeScale = 1.0f;
        UITransitions.Instance?.FadeOut(0.4f, "MainMenu");
    }

    public bool CheckIngredients(string ingredient)
    {
        switch (ingredient)
        {
            case "Arnibal":
                if (arnibalStock <= 0)
                    return false;
                else
                {
                    arnibalStock--;
                    arnibalTxt.SetText($"{arnibalStock}/75");
                    arnibalSlider.value = arnibalStock / 75f;
                }
                break;
            case "Sago":
                if (sagoStock <= 0)
                    return false;
                else
                {
                    sagoStock--;
                    sagoTxt.SetText($"{sagoStock}/75");
                    sagoSlider.value = sagoStock / 75f;
                }
                break;
            case "Taho":
                if (soyaStock <= 0)
                    return false;
                else
                {
                    soyaStock--;
                    soyaTxt.SetText($"{soyaStock}/75");
                    soyaSlider.value = soyaStock / 75f;
                }
                break;
        }

        return true;
    }

    public void Hover() => AudioHandler.instance.PlaySFX("Hover");
    public void Click() => AudioHandler.instance.PlaySFX("Click");
}
