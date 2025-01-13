using UnityEngine;
using UnityEngine.EventSystems;

public class ScoopHandler : MonoBehaviour
{
    public Sprite[] scoops;
    string content;

    public void InitializeScoop(int index, string content)
    {
        GetComponent<UnityEngine.UI.Image>().sprite = scoops[index];
        this.content = content;
    }

    public string GetContent() => content;
}
