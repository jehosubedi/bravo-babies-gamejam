using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float movementSpeed = 8;

    Rigidbody2D rb;
    Vector2 inputVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        inputVector = new Vector2(joystick.Horizontal(),joystick.Vertical());

        rb.MovePosition(rb.position + inputVector * movementSpeed * Time.fixedDeltaTime);
    }
}
