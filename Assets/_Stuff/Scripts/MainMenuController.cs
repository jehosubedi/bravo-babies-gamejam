using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Method for the Start button to transition to the Prep Scene
    public void StartGame()
    {
        // Replace "PrepScene" with the name of your preparation scene
        SceneManager.LoadScene("PrepScene");
    }

    // Method for the Quit button to transition to the End Game scene or quit the application
    public void QuitGame()
    {
        // You can either load an end game scene or quit the application
        Debug.Log("Quitting Game...");
        Application.Quit();  // This will work only in the built application

        // Uncomment below if you have an "EndGame" scene to transition to
        // SceneManager.LoadScene("EndGame");
    }

    // Method for the Settings button to transition to the Settings menu
    public void OpenSettings()
    {
        // Assuming "Settings" is a separate scene or UI panel
        Debug.Log("Opening Settings...");
        SceneManager.LoadScene("Settings");

        // Alternatively, if Settings is a UI panel in the same scene:
        // settingsPanel.SetActive(true);
    }
}
