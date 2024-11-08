using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image BG;
    private Image knob;
    private Vector3 inputVector;
    void Start()
    {
        BG = this.GetComponent<Image>();
        knob = this.transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(BG.rectTransform, data.position, data.pressEventCamera, out pos))
        {
            pos.x = (pos.x / BG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / BG.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2, 0, pos.y * 2);

            inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;
            knob.rectTransform.anchoredPosition = new Vector2(inputVector.x * (BG.rectTransform.sizeDelta.x / 3),
                                                              inputVector.z * (BG.rectTransform.sizeDelta.y / 3));
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }
    public void OnPointerUp(PointerEventData data)
    {
        inputVector = Vector3.zero;
        knob.rectTransform.anchoredPosition = inputVector;
        StopAllCoroutines();
    }
    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxisRaw("Horizontal");
    }
    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxisRaw("Vertical");
    }
    private void OnDisable()
    {
        inputVector = Vector3.zero;
        knob.rectTransform.anchoredPosition = inputVector;
        Debug.Log("Reset Knob");
    }
}
