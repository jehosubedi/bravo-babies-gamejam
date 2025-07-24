using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject overwritePrompt;
    public Material cloudMats;
    public float parallaxRate;

    private void Update()
    {
        cloudMats.mainTextureOffset = new Vector2(cloudMats.mainTextureOffset.x + parallaxRate * Time.deltaTime, 0);
    }
    public void StartGame()
    {
        if(PlayerPrefs.HasKey("GameSave"))
            overwritePrompt.SetActive(true);
        else
            UITransitions.Instance?.FadeOut(0.4f, "Game");
    }
    
    public void ContinueGame()
    {
        UITransitions.Instance?.FadeOut(0.4f, "Game");
    }
    public void OverwriteGame()
    {
        PlayerPrefs.DeleteAll();
        UITransitions.Instance?.FadeOut(0.4f, "Game");
    }
    public void QuitGame() => Application.Quit();
    public void OpenSettings() => SceneManager.LoadScene("Settings",LoadSceneMode.Additive);
    public void OpenWebsite()
    {
        Application.OpenURL("https://bravobabies.jehosubedi.com");
    }

    public void Hover() => AudioHandler.instance.PlaySFX("Hover");
    public void Click() => AudioHandler.instance.PlaySFX("Click");
}
