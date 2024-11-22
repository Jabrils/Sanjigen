using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrl : MonoBehaviour
{
    ctrl_board the_Board;
    public GameObject board;

    public float rotationSpeed = 100f;
    private Vector3 lastMousePosition;

    void Start()
    {
        the_Board = FindObjectOfType<ctrl_board>();
    }

    void Update()
    {
        float rotationX = 0; // .33f;
        float rotationY = 0; // .247f;

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

    void HandleRightClick()
    {
        // Perform a raycast from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object has the RCVR_Board_Pos component or any relevant component
            var boardPos = hit.collider.GetComponent<RCVR_Board_Pos>();
            if (boardPos != null)
            {
                // Access the Piece ID or other properties from the board position
                int pieceId = boardPos.piece.id;
                Debug.Log($"Right-clicked on piece with ID: {pieceId}");

                // Evaluate the board using the control board logic
                the_Board.EvalutePosition(pieceId);
            }
            else
            {
                Debug.Log("Right-clicked on a non-board object.");
            }
        }
        else
        {
            Debug.Log("Right-clicked on empty space.");
        }
    }
}


public static class GM
{
    //public static int total;
    //public static int[] wins = new int[2];
}