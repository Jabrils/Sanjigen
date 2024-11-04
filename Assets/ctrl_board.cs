using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ctrl_board : MonoBehaviour
{
    int turn;
    public int turn_Display => turn % 2 == 0 ? 0 : 1;
    public List<Peice> peice;
    public Material mat_Yellow;
    public Material mat_Blue;
    public Material mat_Outline;
    public Material this_Turn_Mat => new Material[] { mat_Yellow, mat_Blue }[turn_Display];
    internal Dictionary<int, int> board = new Dictionary<int, int>();
    HashSet<string> win_Pos = new HashSet<string>();
    HashSet<int> remaining = new HashSet<int>();
    public TextMeshProUGUI[] txt_Score;
    int[] pts = new int[2];

    // Start is called before the first frame update
    void Awake()
    {
        win_Pos.Add("0,1,2");
        win_Pos.Add("3,4,5");
        win_Pos.Add("6,7,8");
        win_Pos.Add("9,10,11");
        win_Pos.Add("12,13,14");
        win_Pos.Add("15,16,17");
        win_Pos.Add("18,19,20");
        win_Pos.Add("21,22,23");

        win_Pos.Add("0,3,6");
        win_Pos.Add("1,4,7");
        win_Pos.Add("2,5,8");

        win_Pos.Add("6,9,12");
        win_Pos.Add("7,10,13");
        win_Pos.Add("8,11,14");

        win_Pos.Add("12,15,18");
        win_Pos.Add("13,16,19");
        win_Pos.Add("14,17,20");

        win_Pos.Add("0,18,21");
        win_Pos.Add("1,19,22");
        win_Pos.Add("2,20,23");

        win_Pos.Add("11,23,25");

        win_Pos.Add("3,15,24");
        win_Pos.Add("5,17,25");

        win_Pos.Add("2,4,6");
        win_Pos.Add("0,4,8");

        win_Pos.Add("8,10,12");
        win_Pos.Add("6,10,14");

        win_Pos.Add("14,16,18");
        win_Pos.Add("12,16,20");

        win_Pos.Add("0,20,22");
        win_Pos.Add("2,18,22");

        win_Pos.Add("0,12,24");
        win_Pos.Add("6,12,24");

        win_Pos.Add("2,14,25");
        win_Pos.Add("8,20,25");

        //GameObject[] p = GameObject.FindGameObjectsWithTag("Peice");
        Transform[] p = transform.GetComponentsInChildren<Transform>();

        // 
        peice = new List<Peice>();

        // 
        for (int i = 0; i < p.Length; i++)
        {

            Transform pp = p[i].transform;

            // 
            if (pp.tag == "Peice")
            {

                Peice new_Peice = new Peice(this, pp, pp.localPosition + pp.forward * 1, pp.localPosition);
                new_Peice.UnparentBoardPos();

                new_Peice.SetToStart();
                peice.Add(new_Peice);
                remaining.Add(new_Peice.id);
            }
        }

        StartCoroutine(rPos());
    }

    // Check if a player has a winning position
    Point[] CheckForWinner()
    {
        List<Point> points = new List<Point>();

        foreach (var pos in win_Pos)
        {
            // Split the win position string into individual board positions and order them
            int[] positions = pos.Split(',').Select(int.Parse).OrderBy(p => p).ToArray();


            // Check if all three positions in this combination are the same player
            if (board.ContainsKey(positions[0]) && board.ContainsKey(positions[1]) && board.ContainsKey(positions[2]))
            {
                int player = board[positions[0]];

                //print($"{positions[0]}, {positions[1]}, {positions[2]} | {board[positions[0]]}, {board[positions[1]]}, {board[positions[2]]}");

                // Verify that all positions belong to the same player
                if (board[positions[1]] == player && board[positions[2]] == player)
                {
                    Point pt = new Point(player, positions);
                    points.Add(pt);
                }
            }
        }

        // Return -1 if there is no winner
        return points.ToArray();
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < peice.Count; i++)
        {
            peice[i].Update();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectRandomPosition();
        }
    }

    void SelectRandomPosition()
    {
        if (remaining.Count == 0)
        { return; }

        System.Random random = new System.Random();
        int p = remaining.ElementAt(random.Next(remaining.Count));

        SelectPosition(p);
    }

    void SelectPosition(int p)
    {
        if (pts[0] < 3 && pts[1] < 3)
        {
            peice[p].Activate();
            remaining.Remove(p);
        }
    }

    IEnumerator rPos()
    {
        yield return new WaitForSeconds(.1f);
        SelectRandomPosition();
        StartCoroutine(rPos());
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
        Point[] p = CheckForWinner();

        pts = new int[2];

        for (int i = 0; i < p.Length; i++)
        {
            int who = p[i].who;

            pts[who] += 1;
            txt_Score[who].text = $"P{i}\n{pts[who]}";

            // 
            for (int j = 0; j < p[i].pos.Length; j++)
            {
                peice[p[i].pos[j]].Glow();
            }
        }

    }
}

public class Point
{
    public int who;
    public int[] pos;

    public Point(int who, int[] pos)
    {
        this.who = who;
        this.pos = pos;
    }
}