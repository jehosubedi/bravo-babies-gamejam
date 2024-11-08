using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

   
    public void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
