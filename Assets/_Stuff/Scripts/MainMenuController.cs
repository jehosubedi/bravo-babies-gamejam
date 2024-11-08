using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject continueBtn;

    private void Awake() => continueBtn.SetActive(PlayerPrefs.HasKey("GameSave"));

    public void StartGame() => UITransitions.Instance.FadeOut(0.4f, "CityScene");

    public void QuitGame() => Application.Quit();

    public void OpenSettings()
    {
        Debug.Log("Opening Settings...");
        //SceneManager.LoadScene("Settings");
    }
}
