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
        float rotationX = .33f;
        float rotationY = .247f;

        // 
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            rotationX = delta.y * rotationSpeed * Time.deltaTime;
            rotationY = -delta.x * rotationSpeed * Time.deltaTime;

            lastMousePosition = Input.mousePosition;
        }

        board.transform.Rotate(Vector3.up, rotationY, Space.World);
        board.transform.Rotate(-Vector3.right, rotationX, Space.World);
    }
}

public static class GM
{
    //public static int total;
    //public static int[] wins = new int[2];
}