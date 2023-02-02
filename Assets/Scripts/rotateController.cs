using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]

public class rotateController : MonoBehaviour 
{
    private float xRotationSpeed = 5f;
    private float yRotationSpeed = 5f;
    private float zRotationSpeed = 5f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchDeltaPosition = touch.deltaPosition;

            transform.Rotate(Vector3.right, -touchDeltaPosition.y * Time.deltaTime * yRotationSpeed, Space.Self);
            transform.Rotate(Vector3.up, -touchDeltaPosition.x * Time.deltaTime * xRotationSpeed, Space.Self);
            transform.Rotate(Vector3.forward, touchDeltaPosition.x * Time.deltaTime * zRotationSpeed, Space.Self);
        }
    }	
}