using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public MobSpawnController spawnController;
    public CanvasGroup blackScreen;
    public GameObject resultScreen;
    public GameObject pauseMenu;

    [Header("Calendar")]
    public TMP_Text dayTxt;
    public TMP_Text dayNameTxt;
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
    public TMP_Text ResultTxt;
    public GameObject currentMoney;
    public TMP_Text currentMoneyTxt;
    public GameObject sold;
    public TMP_Text soldTxt;
    public GameObject earned;
    public TMP_Text earnedTxt;
    public GameObject spent;
    public TMP_Text spentTxt;
    public GameObject total;
    public TMP_Text totalTxt;
    public GameObject again;
    public GameObject menu;

    [Header("Lights")]
    public Light2D globalLight;
    public GameObject[] lights;
    public Gradient globalGradient;
    private float gradientValue = 0;
    private bool lightsActivated = false;

    private int arnibalStock = 75;
    private int soyaStock = 75;
    private int sagoStock = 75;

    private float timer = 0;
    private string timerString = "AM";
    private int hour = 5;
    private bool dayEnded = false;
    private int earnedCash;
    private int currentCash;
    private int cashSpent;

    private int currentDay = 1;

    private string[] dayNames = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
    int served;
    private void Start()
    {
        currentDay = (PlayerPrefs.HasKey("Day") ? PlayerPrefs.GetInt("Day") : 1);
        dayTxt.SetText($"{currentDay}");
        dayNameTxt.text = dayNames[(currentDay - 1) % 7];
        currentCash = PlayerPrefs.HasKey("Cash") ? PlayerPrefs.GetInt("Cash") : 0;
        cashTxt.SetText(currentCash.ToString());
        timeTxt.SetText($"{hour}:00 {timerString}");
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
            {
                timer += Time.deltaTime;
                gradientValue += Time.deltaTime;
                globalLight.color = globalGradient.Evaluate(gradientValue / 150);
            }
            else
                EndDay();

            if (hour == 18 && !lightsActivated)
            {
                lightsActivated = true;
                lights[0].SetActive(true);
                List<int> lightIndicis = new();
                int lightAmount = Random.Range(1, lights.Length);
                for (int i = 0; i < lightAmount; i++)
                {
                    int r = Random.Range(1, lights.Length);
                    if(!lightIndicis.Contains(r))
                        lightIndicis.Add(r);
                }

                foreach (int r in lightIndicis)
                    lights[r].SetActive(true);
            }  

            if (timer >= 10f)
            {
                hour++;
                hour = Mathf.Clamp(hour, 5, 20);
                timeTxt.SetText($"{hour}:00 {timerString}");
                timer = 0;
            }

            if (hour >= 12)
            {
                timerString = "PM";
                endBtn.SetActive(true);
            }
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
        earnedCash += 15;
        cashTxt.SetText((earnedCash + currentCash).ToString());
        AudioHandler.instance?.PlaySFX("Coin");
    }
    public void ConsumeCash(int amount)
    {
        currentCash -= amount;
        cashSpent += amount;
        cashTxt.SetText((currentCash-amount).ToString());
        AudioHandler.instance?.PlaySFX("Coin");
    }

    public void EndDay()
    {
        dayEnded = true;
        spawnController.EndFunction();
        AudioHandler.instance?.BGMSource.Stop();
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

            ResultTxt.text = $"RESULTS\r\n<size=35>(Day {currentDay})</size>";

            resultScreen.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            currentMoney.SetActive(true);
            currentMoneyTxt.SetText(currentCash+"");

            yield return new WaitForSeconds(0.6f);
            sold.SetActive(true);
            soldTxt.text = served.ToString();
            //float currentSold = 0;
            //var soldRate = Mathf.Abs(served - currentSold) / 0.8f;
            //while(currentSold != served)
            //{
            //    currentSold = Mathf.MoveTowards(currentSold, served, soldRate * Time.deltaTime);
            //    soldTxt.text = ((int)currentSold).ToString();
            //    yield return null;
            //}
            yield return new WaitForSeconds(0.6f);

            earned.SetActive(true);
            earnedTxt.text = earnedCash.ToString();
            //float currentEarned = 0;
            //var earnRate = Mathf.Abs(currentCash - currentEarned) / 0.8f;
            //while(currentEarned != currentCash)
            //{
            //    currentEarned = Mathf.MoveTowards(currentEarned, currentCash, earnRate * Time.deltaTime);
            //    earnedTxt.text = ((int)currentEarned).ToString();
            //    yield return null;
            //}
            yield return new WaitForSeconds(0.6f);

            spent.SetActive(true);
            spentTxt.text = cashSpent.ToString();
            //float currentTotal = (totalCash + currentCash) - cashSpent;
            //var totalRate = currentCash == 0 ? (Mathf.Abs(currentTotal - currentCash) / 0.8f) : 0;
            //while (currentTotal != (totalCash))
            //{   
            //    currentTotal = Mathf.MoveTowards(currentTotal, totalCash, totalRate * Time.deltaTime);
            //    totalTxt.text = ((int)currentTotal).ToString();
            //    yield return null;
            //}
            yield return new WaitForSeconds(0.6f);

            total.SetActive(true);
            int totalCash = currentCash + earnedCash;
            totalTxt.text = totalCash.ToString();
            yield return new WaitForSeconds(0.6f);
            again.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            menu.SetActive(true);

            PlayerPrefs.SetInt("Cash", totalCash);
            int d = PlayerPrefs.HasKey("Day") ? PlayerPrefs.GetInt("Day") : 1;
            PlayerPrefs.SetInt("Day", d+=1);
            PlayerPrefs.SetInt("GameSave", 1);
            PlayerPrefs.Save();
            yield return null;
        }
    }

    public void OpenSettings() => UnityEngine.SceneManagement.SceneManager.LoadScene("Settings", UnityEngine.SceneManagement.LoadSceneMode.Additive);

    public void ExitGame()
    {
        Time.timeScale = 1.0f;
        UITransitions.Instance?.FadeOut(0.4f, "MainMenu");
    }

    public void NextDay()
    {
        UITransitions.Instance?.FadeOut(0.4f, "Game");
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
