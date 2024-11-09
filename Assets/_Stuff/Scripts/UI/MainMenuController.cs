using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject overwritePrompt;
    public void StartGame()
    {
        if(PlayerPrefs.HasKey("GameSave"))
            overwritePrompt.SetActive(false);
        else
            UITransitions.Instance?.FadeOut(0.4f, "CityScene");
    }
    public void ContinueGame()
    {
        // LOAD EXISTING SAVE BEFORE LOADING GAME
        UITransitions.Instance?.FadeOut(0.4f, "CityScene");
    }
    public void OverwriteGame()
    {
        // DELETE EXISTING SAVE AND RESET EVERYTHING BACK TO DEFAULT
        UITransitions.Instance?.FadeOut(0.4f, "CityScene");
    }
    public void QuitGame() => Application.Quit();
    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        //SceneManager.LoadScene("Settings");
    }
}
