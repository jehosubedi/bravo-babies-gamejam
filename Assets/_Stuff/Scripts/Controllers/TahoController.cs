using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TahoController : MonoBehaviour
{
    public PlayerController controller;
    public GameObject cupPrefab;
    public GameObject handPrefab;
    public Transform cupPosition;
    public Transform garapon;
    public Transform handPosition;
    public TMP_Text orderCount;

    GameObject currentCup;
    GameObject currentOrder;

    public void GetCup()
    {
        if (currentCup == null)
        {
            var go = Instantiate(cupPrefab, garapon);
            go.transform.position = cupPosition.position;
            currentCup = go;
        }
        else
            return;
    }

    public void AddOrder(int c)
    {
        //var go = Instantiate(handPrefab, garapon);
        //go.transform.position = handPosition.position;
        //go.GetComponent<OrderHelper>().Initialize(npc, controller);
        //currentOrder = go;
        orderCount.text = $"Orders: {c}";
    }

    public void FulfillOrder()
    {
        //Destroy(currentOrder);
        controller.Fulfill();
    }
}
