using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCVR_Board_Pos : MonoBehaviour
{
    public Material mat_Transparent; // Default material
    public Material mat_Display; // Hover material

    Renderer objectRenderer;
    public GameObject obj_Peice;
    Peice peice;


    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        // Set to default material initially
        objectRenderer.material = mat_Transparent;

        int id = int.Parse(obj_Peice.name.Split(" ")[1]);

        ctrl_board p = FindObjectOfType<ctrl_board>();

        //print($"{obj_Peice.name} | {id}");

        peice = p.peice[id];
    }

    public void Unparent()
    {
        transform.SetParent(GameObject.Find("board").transform);
    }

    void OnMouseEnter()
    {
        // Change to hover material when mouse enters
        if (!peice.board.ActiveBoardState(peice.id) && peice.board.game_Active)
        {
            objectRenderer.material = mat_Display;
        }
    }

    void OnMouseDown()
    {
        //print(peice.id);
        if (!peice.board.ActiveBoardState(peice.id) && peice.board.game_Active)
        {
            peice.board.SelectPosition(peice.id);
            objectRenderer.material = mat_Transparent; // Change back to default material when mouse exits
        }
    }

    void OnMouseExit()
    {
        objectRenderer.material = mat_Transparent; // Change back to default material when mouse exits
    }
}