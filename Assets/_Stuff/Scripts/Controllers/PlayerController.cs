using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float movementSpeed = 8;
    public GameObject modal;
    public TahoController tahoController;

    Rigidbody2D rb;
    Vector2 inputVector;
    CameraController cam;
    List<AIController> storeQueue = new List<AIController>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main.GetComponent<CameraController>();
    }
    public void Update()
    {
        if (storeQueue.Count == 0)
            inputVector = new Vector2(joystick.Horizontal(), joystick.Vertical());
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputVector * movementSpeed * Time.fixedDeltaTime);
    }

    public bool Queue(AIController customer)
    {
        var queued = false;
        if (storeQueue.Count < 3)
        {
            storeQueue.Add(customer);
            cam.ToggleOffset(true);
            queued = true;
            inputVector = Vector2.zero;
            modal.SetActive(true);
            tahoController.AddOrder(storeQueue.Count);
        }

        return queued;
    }

    public void Fulfill(/*AIController target*/)
    {
        storeQueue.Remove(storeQueue[storeQueue.Count-1]);
        tahoController.AddOrder(storeQueue.Count);
        if (storeQueue.Count == 0)
        {
            cam.ToggleOffset(false);
            modal.SetActive(false);
        }
    }
}
