using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 6f;
    public float sideSpeed = 8f;
    public float trackHalfWidth = 4f;

    void Update()
    {
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;

        float x = transform.position.x;
        x += Input.GetAxis("Horizontal") * sideSpeed * Time.deltaTime;
        if (Input.GetMouseButton(0))
            x += Input.GetAxis("Mouse X") * 0.5f;

        x = Mathf.Clamp(x, -trackHalfWidth, trackHalfWidth);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}