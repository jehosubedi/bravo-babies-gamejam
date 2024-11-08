using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float movementSpeed = 8;

    Rigidbody2D rb;
    Vector2 inputVector;
    CameraController cam;
    int storeQueue;
    bool selling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main.GetComponent<CameraController>();
    }
    public void Update()
    {
        if (!selling)
            inputVector = new Vector2(joystick.Horizontal(), joystick.Vertical());

        if (Input.GetKeyDown(KeyCode.Space))
            ToggleStore(1);
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ToggleStore(-1);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputVector * movementSpeed * Time.fixedDeltaTime);
    }

    public void ToggleStore(int queue)
    {
        storeQueue += queue;
        if (storeQueue > 0)
            selling = true;
        else
            selling = false;
        
        cam.ToggleOffset(selling);
    }
}
