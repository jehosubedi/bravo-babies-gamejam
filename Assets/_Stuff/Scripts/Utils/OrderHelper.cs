using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderHelper : MonoBehaviour
{
    public Slider patienceMeter;
    public Image patienceFill;

    [Header("Ingredients")]
    public GameObject ingredient;
    public Transform ingredientParent;
    public Sprite[] ingredientsSprites;

    private PlayerController p;
    private AIController controller;

    private int soya, arnibal, sago;
    private bool initialized = false;
    private float timer = 15;

    public void Initialize(AIController owner, PlayerController player)
    {
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
                ing.GetComponentInChildren<Image>().sprite = ingredientsSprites[0];
                ing.GetComponentInChildren<TMP_Text>().SetText($"x{sago}");
                ing.SetActive(true);
            }

            if (arnibal > 0)
            {
                var ing = Instantiate(ingredient, ingredientParent);
                ing.GetComponentInChildren<Image>().sprite = ingredientsSprites[1];
                ing.GetComponentInChildren<TMP_Text>().SetText($"x{arnibal}");  
                ing.SetActive(true);
            }

            if (soya > 0)
            {
                var ing = Instantiate(ingredient, ingredientParent);
                ing.GetComponentInChildren<Image>().sprite = ingredientsSprites[2];
                ing.GetComponentInChildren<TMP_Text>().SetText($"x{soya}");
                ing.SetActive(true);
            }

            initialized = true;
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            patienceMeter.value = timer;
            if (timer > 7 && timer < 8)
                patienceFill.color = Color.yellow;
            if (timer < 0.7f)
                patienceFill.color = Color.red;
        }
        else
            Unfulfill();
    }

    public int GetSago() => sago;
    public int GetArnibal() => arnibal;
    public int GetSoya() => soya;

    public void Fulfill()
    {
        p.Fulfill(controller);
        Destroy(gameObject);
    }

    public void Unfulfill()
    {
        p.Unfulfill(controller);
        Destroy(gameObject);
    }
}
