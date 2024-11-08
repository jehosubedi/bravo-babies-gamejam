using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    Vector3 velocity;
    Vector3 offset = new Vector3(0, 0, -10);

    public void FixedUpdate() => transform.position = Vector3.SmoothDamp(transform.position,target.position + offset, ref velocity, 0.1f);
    public void ToggleOffset(bool selling) => offset.x = selling ? -10 : 0;
}
