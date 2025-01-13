using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderHelper : MonoBehaviour
{
    PlayerController p;
    AIController controller;
    public Sprite[] hands;

    [Header("Ingredients")]
    public GameObject ingredient;
    public Transform ingredientParent;
    public Sprite[] ingredientsSprites;


    int soya, arnibal, sago;
    bool initialized = false;

    public void Initialize(AIController owner, PlayerController player)
    {
        GetComponent<Image>().sprite = hands[Random.Range(0, hands.Length)];

        if (!initialized)
        {
            controller = owner;
            p = player;

            for (int i = 0; i < Random.Range(3, 6); i++)
            {
                var rVal = Random.value;

                if (rVal <= 0.33)
                    sago++;
                else if (rVal > 0.33 && rVal <= 0.66)
                    arnibal++;
                else if (rVal > 0.66)
                    soya++;
            }

            if (sago > 0)
            {
                var ing = Instantiate(ingredient, ingredientParent);
                ing.GetComponent<Image>().sprite = ingredientsSprites[0];
                ing.GetComponentInChildren<TMP_Text>().SetText($"x{sago}");
                ing.SetActive(true);
            }

            if (arnibal > 0)
            {
                var ing = Instantiate(ingredient, ingredientParent);
                ing.GetComponent<Image>().sprite = ingredientsSprites[1];
                ing.GetComponentInChildren<TMP_Text>().SetText($"x{arnibal}");
                ing.SetActive(true);
            }

            if (soya > 0)
            {
                var ing = Instantiate(ingredient, ingredientParent);
                ing.GetComponent<Image>().sprite = ingredientsSprites[2];
                ing.GetComponentInChildren<TMP_Text>().SetText($"x{soya}");
                ing.SetActive(true);
            }

            initialized = true;
        }
    }

    public int GetSago() => sago;
    public int GetArnibal() => arnibal;
    public int GetSoya() => soya;

    public void Fulfill()
    {
        p.Fulfill(controller);
        Destroy(gameObject);
    }
}
