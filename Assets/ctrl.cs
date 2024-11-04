using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrl : MonoBehaviour
{
    public GameObject board;

    public float rotationSpeed = 100f;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = delta.y * rotationSpeed * Time.deltaTime;
            float rotationY = -delta.x * rotationSpeed * Time.deltaTime;

            board.transform.Rotate(Vector3.up, rotationY, Space.World);
            board.transform.Rotate(-Vector3.right, rotationX, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}
