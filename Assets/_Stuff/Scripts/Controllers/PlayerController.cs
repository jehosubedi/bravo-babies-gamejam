using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float movementSpeed = 8;
    public GameObject modal;
    public TahoController tahoController;
    public HUDController hudController;
    public CinemachinePositionComposer cam;

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private Animator anim;
    private List<AIController> storeQueue = new List<AIController>();

    bool paused = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    public void Update()
    {
        if (storeQueue.Count == 0)
            inputVector = new Vector2(joystick.Horizontal(), joystick.Vertical());

        anim.SetBool("IsMoving", inputVector.magnitude > 0);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                hudController.PauseGame();
                paused = true;
            }
            else
            {
                hudController.ContinueGame();
                paused = false;
            }
        }    
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputVector * movementSpeed * Time.fixedDeltaTime);
        anim.SetFloat("X", inputVector.x);
        anim.SetFloat("Y", inputVector.y);
    }

    public bool Queue(AIController customer)
    {
        var queued = false;
        if (storeQueue.Count < 3)
        {
            storeQueue.Add(customer);
            cam.TargetOffset.x = 2;
            queued = true;
            inputVector = Vector2.zero;
            if(!modal.activeSelf)
                AudioHandler.instance?.PlaySFX("Open");
            modal.SetActive(true);
            tahoController.AddOrder(customer);
        }

        return queued;
    }

    public void Fulfill(AIController target)
    {
        storeQueue.Remove(storeQueue[storeQueue.Count-1]);
        target.FulfillOrder();
        hudController.UpdateCash();
        //Increase cash here
        if (storeQueue.Count == 0)
        {
            cam.TargetOffset.x = 0;
            modal.SetActive(false);
            AudioHandler.instance?.PlaySFX("Close");
        }
    }

    public void Unfulfill(AIController target)
    {
        storeQueue.Remove(storeQueue[storeQueue.Count - 1]);
        target.UnfulfilleOrder();
        if (storeQueue.Count == 0)
        {
            cam.TargetOffset.x = 0;
            modal.SetActive(false);
            AudioHandler.instance?.PlaySFX("Close");
        }
    }
}
