using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float movementSpeed = 8;

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
        }

        return queued;
    }

    public void Fulfill(AIController target)
    {
        storeQueue.Remove(target);
        if (storeQueue.Count == 0)
            cam.ToggleOffset(false);
    }
}
