using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TahoController : MonoBehaviour
{
    public PlayerController controller;
    public HUDController hud;
    public Canvas canvas;
    public GameObject cupPrefab;
    public GameObject orderPrefab;
    public Transform cupPosition;
    public Transform garapon;
    public Transform[] handPositions;
    public TMP_Text orderCount;
    public GameObject scoop;

    [Header("Ingredients")]
    public GameObject[] ingredients;

    public Sprite stirred;

    [Header("Interactions")]
    public GameObject[] buttons;

    [Header("Indicators")]
    public TMP_Text arnibalTxt;
    public TMP_Text soyaTxt;
    public TMP_Text sagoTxt;

    GameObject currentCup;
    GameObject currentOrder;
    RectTransform currentScoop;

    int arnibalInt;
    int soyaInt;
    int sagoInt;

    int cupIndex;

    GraphicRaycaster raycaster;

    private void Start()
    {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {
        if (currentScoop != null && Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 pos);
            currentScoop.anchoredPosition = pos;
        }
        else if (Input.GetMouseButtonUp(0) && currentScoop != null)
        {
            GameObject detected = GetUIUnderMouse();

            if (detected != null && detected.Equals(currentCup))
            {
                buttons[1].SetActive(true);
                buttons[0].SetActive(true);

                if(currentCup.TryGetComponent(out CupHandler cup))

                switch (currentScoop.GetComponent<ScoopHandler>().GetContent())
                {   case "Arnibal":
                        arnibalInt++;
                        arnibalTxt.SetText($"x{arnibalInt}");
                        if(!cup.hasArnibal)
                            {
                                cup.hasArnibal = true;
                                cup.AddIngredient(0);
                            }
                        break;
                    case "Sago":
                        sagoInt++;
                        sagoTxt.SetText($"x{sagoInt}");
                        if (!cup.hasSago)
                            {
                                cup.hasSago = true;
                                cup.AddIngredient(1);
                            }
                        break;
                    case "Taho":
                        soyaInt++;
                        soyaTxt.SetText($"x{soyaInt}");
                        if (!cup.hasSoya)
                            {
                                cup.hasSoya = true;
                                cup.AddIngredient(2);
                            }
                        break;
                } 
            }
            Destroy(currentScoop.gameObject);
        }
    }

    public void GetCup()
    {
        if (currentCup == null)
        {
            var go = Instantiate(cupPrefab, garapon);
            go.transform.position = cupPosition.position;
            currentCup = go;
            cupIndex = -1;

            for (int i = 0; i < ingredients.Length; i++)
                ingredients[i].SetActive(true);
        }
        else
            return;
    }

    public void ScoopIngredient(string ingredient)
    {
        if (!hud.CheckIngredients(ingredient))
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 pos))
        {
            var s = Instantiate(scoop, transform);
            s.GetComponent<RectTransform>().anchoredPosition = pos;
            currentScoop = s.GetComponent<RectTransform>();
            ScoopHandler handler = s.GetComponent<ScoopHandler>();

            switch (ingredient)
            {
                case "Arnibal":
                    handler.InitializeScoop(0, ingredient);
                    break;
                case "Sago":
                    handler.InitializeScoop(1, ingredient);
                    break;
                case "Taho":
                    handler.InitializeScoop(2, ingredient);
                    break;
            }
        }
    }



    public void AddOrder(AIController npc)
    {
        var go = Instantiate(orderPrefab, garapon);
        for (int i = 0; i < handPositions.Length; i++)
        {
            if (handPositions[i].childCount <= 0)
            {
                go.transform.position = handPositions[i].position;
                go.transform.SetParent(handPositions[i]);
                go.GetComponent<OrderHelper>().Initialize(npc, controller);
            }
        }
        //go.GetComponent<OrderHelper>().Initialize(npc, controller);
        //currentOrder = go;
    }

    public void ScrapOrder()
    {
        sagoInt = 0; soyaInt = 0; arnibalInt = 0;
        sagoTxt.SetText("x0"); soyaTxt.SetText("x0"); arnibalTxt.SetText("x0");

        cupIndex = -1;
        if (currentCup != null) Destroy(currentCup);

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].SetActive(false);
        for (int i = 0; i < ingredients.Length; i++)
            ingredients[i].SetActive(false);
    }

    public void MixOrder()
    {
        currentCup.GetComponent<Image>().sprite = stirred;
        currentCup.GetComponent<CupHandler>().MixIngredients();
        currentCup.GetComponent<CupHandler>().ReadyToServe(this, raycaster, canvas, sagoInt, arnibalInt, soyaInt);
        buttons[0].SetActive(false);
        // Make the cup serviceable
    }

    private GameObject GetUIUnderMouse()
    {
        // Create a PointerEventData with the current mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Perform the raycast and store the results
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointerData, raycastResults);

        // Return the first GameObject hit, if any
        if (raycastResults.Count > 0)
        {
            return raycastResults[0].gameObject;
        }

        return null;
    }
}
