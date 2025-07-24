using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CupHandler : MonoBehaviour
{
    TahoController controller;
    RectTransform rectTransform;
    RectTransform parentRect;
    GraphicRaycaster raycaster;
    Canvas canvas;

    public Image[] partsImage;
    public Sprite[] partsSprite;

    bool isReady = false;
    Vector2 defPos;
    bool dragging = false;

    int sago, arnibal, soya;
    internal bool hasSago, hasArnibal, hasSoya;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        defPos = rectTransform.anchoredPosition;
    }
    public void ReadyToServe(TahoController controller, GraphicRaycaster rc, Canvas c, int sago, int arnibal, int soya)
    {
       this.controller = controller;
        raycaster = rc;
        canvas = c;
        isReady = true;
        this.sago = sago;
        this.arnibal = arnibal;
        this.soya = soya;
    }

    private void Update()
    {
        if(isReady && raycaster != null)
        {
            if (Input.GetMouseButton(0))
            {
                GameObject detected = GetUIUnderMouse();
                if (detected == gameObject && !dragging)
                {
                    dragging = true;
                    GetComponent<Image>().raycastTarget = false;
                }

                if (dragging)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, canvas.worldCamera, out Vector2 pos);
                    rectTransform.anchoredPosition = pos;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                GameObject detected = GetUIUnderMouse();

                if (detected.GetComponent<OrderHelper>() != null)
                {
                    if(detected.GetComponent<OrderHelper>().GetSago() == sago &&
                       detected.GetComponent<OrderHelper>().GetArnibal() == arnibal &&
                       detected.GetComponent<OrderHelper>().GetSoya() == soya)
                    {
                        //fullfil order!
                        detected.GetComponent<OrderHelper>().Fulfill();
                        Destroy(gameObject);
                        controller.ScrapOrder();
                    }
                    else
                    {
                        dragging = false;
                        rectTransform.anchoredPosition = defPos;
                        GetComponent<Image>().raycastTarget = true;
                    }
                }
                else
                {
                    rectTransform.anchoredPosition = defPos;
                    dragging = false;
                    GetComponent<Image>().raycastTarget = true;
                }
            }
        }
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

    public void AddIngredient(int id)
    {
        int targetPart = 0;
        bool hasSlot = true;

        for (int i = 0; i < partsImage.Length; i++)
        {
            if (partsImage[i].sprite == null)
            {
                targetPart = i;
                hasSlot = true;
                break;
            }
            else
                hasSlot = false;
        }

        if (!hasSlot) return;
        
        partsImage[targetPart].sprite = partsSprite[id];
        partsImage[targetPart].enabled = true;
    }

    public void MixIngredients()
    {
        for (int i = 0; i < partsImage.Length; i++)
        {
            partsImage[i].enabled = false;
        }
    }

}
