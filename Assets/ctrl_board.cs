using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class ctrl_board : MonoBehaviour
{
    int turn;
    public int turn_Display => turn % 2 == 0 ? 0 : 1;
    public List<Peice> peice;
    public Material mat_Yellow;
    public Material mat_Blue;
    public Material mat_Outline;
    public Material this_Turn_Mat => new Material[] {mat_Yellow, mat_Blue }[turn_Display];
    internal Dictionary<int, int> board = new Dictionary<int, int>();

    // Start is called before the first frame update
    void Awake()
    {
        //GameObject[] p = GameObject.FindGameObjectsWithTag("Peice");
        Transform[] p = transform.GetComponentsInChildren<Transform>();

        // 
        peice = new List<Peice>();

        // 
        for (int i = 0; i < p.Length; i++)
        {
            Transform pp = p[i].transform;

            if (pp.tag == "Peice")
            {
                Peice new_Peice = new Peice(this, pp, pp.localPosition + pp.forward * 1, pp.localPosition);
                new_Peice.UnparentBoardPos();

                new_Peice.SetToStart();
                peice.Add(new_Peice);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < peice.Count; i++)
        {
            peice[i].Update();
        }
    }
    public bool ActiveBoardState(int boardIndex)
    {
        return board.ContainsKey(boardIndex);
    }

    public void IncrementTurn()
    {
        turn++;
    }

    public void AddToBoard(int pos)
    {
        board.Add(pos, turn_Display);
    }
}
