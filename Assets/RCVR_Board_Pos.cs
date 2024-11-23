using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCVR_Board_Pos : MonoBehaviour
{
    public Material mat_Transparent; // Default material
    public Material mat_Display; // Hover material

    Renderer objectRenderer;
    public GameObject obj_Peice;
    internal Peice piece;


    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        // Set to default material initially
        objectRenderer.material = mat_Transparent;

        int id = int.Parse(obj_Peice.name.Split(" ")[1]);

        ctrl_board p = FindObjectOfType<ctrl_board>();

        //print($"{obj_Peice.name} | {id}");

        piece = p.peice[id];
    }

    public void Unparent()
    {
        transform.SetParent(GameObject.Find("board").transform);
    }

    public void Update()
    {
      
    }

    void OnMouseEnter()
    {
        // Change to hover material when mouse enters
        if (!piece.board.ActiveBoardState(piece.id) && piece.board.game_Active)
        {
            objectRenderer.material = mat_Display;
        }
    }

    void OnMouseDown()
    {

        //print(peice.id);
        if (piece.board.player[piece.board.turn_Display] != ctrl_board.Player.self)
        {
            return;
        }

        if (!piece.board.ActiveBoardState(piece.id) && piece.board.game_Active)
        {
            piece.board.SelectPosition(piece.id);
            objectRenderer.material = mat_Transparent; // Change back to default material when mouse exits
        }
    }

    void OnMouseExit()
    {
        objectRenderer.material = mat_Transparent; // Change back to default material when mouse exits
    }
}