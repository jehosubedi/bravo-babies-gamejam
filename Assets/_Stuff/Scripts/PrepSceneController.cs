using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PrepSceneController : MonoBehaviour
{
    public GameObject qteIcon; // Assign the QTE icon in the Unity editor
    public float showTime = 1.0f; // How long the icon stays visible
    public float hideTime = 2.0f; // Time between icon appearances
    private bool iconIsActive = false;
    private float timer;

    void Start()
    {
        // Initialize by hiding the QTE icon
        qteIcon.SetActive(false);
        StartCoroutine(ShowQTEIcon()); // Start the QTE coroutine
    }

    void Update()
    {
        if (iconIsActive && Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            HandleQTEClick();
        }
    }

    private void HandleQTEClick()
    {
        Debug.Log("Successful Click!");
        // Add logic here for successful preparation actions, like score or animation
        qteIcon.SetActive(false);
        iconIsActive = false;
        StartCoroutine(ShowQTEIcon()); // Reset for the next QTE
    }

    private IEnumerator ShowQTEIcon()
    {
        yield return new WaitForSeconds(hideTime); // Wait before showing icon
        qteIcon.SetActive(true); // Show the QTE icon
        iconIsActive = true;
        timer = showTime;

        // Wait for the time limit, then hide icon if not clicked
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;

            if (!iconIsActive) // Exit if clicked
                yield break;
        }

        // Hide the icon if time ran out
        qteIcon.SetActive(false);
        iconIsActive = false;
        Debug.Log("Missed!");
        StartCoroutine(ShowQTEIcon()); // Restart QTE
    }
}
