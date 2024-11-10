using UnityEngine;

public class OrderHelper : MonoBehaviour
{
    PlayerController p;
    AIController controller;

    public void Initialize(AIController owner, PlayerController player)
    {
        controller = owner;
        p = player;
    }

    //public void Fulfill()
    //{
    //    p.Fulfill(controller);
    //    Destroy(gameObject);
    //}
}
