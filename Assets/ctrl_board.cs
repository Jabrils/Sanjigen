using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ctrl_board : MonoBehaviour
{
    public enum Game_Type { self, aivai, vsH};
    public Game_Type game_Type;
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
    public TextMeshProUGUI[] txt_Wins;
    int[] pts = new int[2];
    internal bool game_Active => pts[0] < 3 && pts[1] < 3;
    HashSet<int> corners = new HashSet<int> { 0, 2, 6, 8, 12, 14, 18, 20 }; // 6
    HashSet<int> edges = new HashSet<int> { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23 }; // 3
    HashSet<int> centers = new HashSet<int> { 4, 10, 16, 22, 24, 25 }; // 4
    Dictionary<int, int> base_Value = new Dictionary<int, int>
    {
        { 0, 6},
        { 1, 3},
        { 2, 6},
        { 3, 3},
        { 4, 4},
        { 5, 3},
        { 6, 6},
        { 7, 3},
        { 8, 6},
        { 9, 3},
        { 10, 4},
        { 11, 3},
        { 12, 6},
        { 13, 3},
        { 14, 6},
        { 15, 3},
        { 16, 4},
        { 17, 3},
        { 18, 6},
        { 19, 3},
        { 20, 6},
        { 21, 3},
        { 22, 4},
        { 23, 3},
        { 24, 4},
        { 25, 4},
    };

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
        win_Pos.Add("9,21,24");

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
        win_Pos.Add("6,18,24");

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

        // 
        if (game_Type == Game_Type.aivai)
        {
            StartCoroutine(rPos2());
        }
    }

    public float EvalutePosition(int pos)
    {
        // List to hold all the lines (win conditions) affected by the given position
        List<int[]> affected = new List<int[]>();

        // Iterate through all possible win conditions stored in win_Pos
        foreach (var value in win_Pos)
        {
            // Split the current win condition (stored as a comma-separated string) into a string array
            string[] val = value.Split(',');

            // Convert the string array into an integer array for numerical processing
            int[] intArray = Array.ConvertAll(val, int.Parse);

            // Check if the current position is part of this win condition
            bool contains = intArray.Contains(pos);

            // If the current position is part of this win condition, add it to the affected list
            if (contains)
            {
                affected.Add(intArray);
            }
        }

        // Variable to store the overall evaluation of the given position
        int oOverall = 0;

        // Iterate through all the affected line
        for (int i = 0; i < affected.Count; i++)
        {
            // Variable to store the cumulative value for this particular line
            int overall = 0;

            int scalar = 0;

            // Iterate through all positions in the current line
            for (int j = 0; j < affected[i].Length; j++)
            {
                int current_Pos = affected[i][j];
                
                overall += base_Value[pos];

                // Check if the position in the current line is present on the board
                if (board.ContainsKey(current_Pos))
                {
                    // Determine if the position is owned by the current player (true) or the opponent (false)
                    bool mine = turn_Display == board[current_Pos];
                    scalar += mine ? 1 : 0;
                    //print($"{affected[i][0]},{affected[i][1]},{affected[i][2]} | {current_Pos} | {mine}");

                    // Print debug information about the current turn, position, and evaluation
                    //print($"TURN: {turn_Display} | POS: {current_Pos} | {(mine ? "1" : "-1")} * {base_Value[current_Pos]}");


                    // Calculate the value for the position: positive if owned by the current player, negative if owned by the opponent
                    int value = (mine ? 1 : -1) * base_Value[current_Pos];

                    // Add the value to the cumulative score for this win condition
                    overall += value;
                }
            }
            scalar = (int)MathF.Max(1, scalar);
            //print(scalar);

            // Add the cumulative value of this line to the overall evaluation
            oOverall += overall * scalar;
        }

        // Print the overall evaluation value for debugging
        //print($"oO: {oOverall}");

        // Calculate and print the average evaluation for the given position
        //print($"Value for Pos {pos}: {(float)oOverall / affected.Count}");

        // Return the average evaluation for the given position
        return Mathf.Abs((float)oOverall / affected.Count);
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

    void Start()
    {
        for (int i = 0; i < txt_Wins.Length; i++)
        {
            int w = PlayerPrefs.GetInt($"wins_{i}");
            int t = PlayerPrefs.GetInt($"total");

            txt_Wins[i].text = $"{w:n0} / {t:n0}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < peice.Count; i++)
        {
            peice[i].Update();
        }

        if (game_Type == Game_Type.vsH && turn_Display == 0)
        {
            SelectEvaluatedPosition();
        }

        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    SelectRandomPosition();
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Dictionary<int, float> evaluation = new Dictionary<int, float>();

        //    foreach (int pos in remaining)
        //    {
        //        float grab = EvalutePosition(pos);

        //        evaluation.Add(pos, grab);
        //    }

        //    Dictionary<int, float> ordered = evaluation.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        //    print($"I SELECTED: {ordered.Keys.First()}");
        //}
    }

    void SelectRandomPosition()
    {
        if (remaining.Count == 0)// || turn_Display == 0)
        {
            AddWins();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        System.Random random = new System.Random();
        int p = remaining.ElementAt(random.Next(remaining.Count));

        SelectPosition(p);
    }

    void AddWins()
    {
        int t = PlayerPrefs.GetInt($"total");

        PlayerPrefs.SetInt("total", t + 1);

        // 
        for (int i = 0; i < 2; i++)
        {
            int w = PlayerPrefs.GetInt($"wins_{i}");

            PlayerPrefs.SetInt($"wins_{i}", w + pts[i] / 3);
        }
    }

    public void SelectPosition(int p)
    {
        if (game_Active)
        {
            peice[p].Activate();
            remaining.Remove(p);
        }
        else
        {
            AddWins();
            //print($"REG: {GM.wins[0]} : {GM.wins[1]} | {pts[0]} : {pts[1]}");

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator rPos()
    {
        //yield return new WaitForEndOfFrame();
        SelectRandomPosition();
        yield return new WaitForSeconds(2f);
        StartCoroutine(rPos());
    }

    IEnumerator rPos2()
    {
        //yield return new WaitForEndOfFrame();
        SelectEvaluatedPosition();
        yield return new WaitForSeconds(2f);
        StartCoroutine(rPos2());
    }

    void SelectEvaluatedPosition()
    {
        Dictionary<int, float> evaluation = new Dictionary<int, float>();

        foreach (int pos in remaining)
        {
            float grab = EvalutePosition(pos);

            evaluation.Add(pos, grab);
        }

        // 
        Dictionary<int, float> ordered = evaluation.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        //// 
        //for (int i = 0; i < ordered.Count; i++)
        //{
        //    print($"{ordered.Keys.ElementAt(i)} : {ordered[i]}");
        //}

        int pos_I_Select = ordered.Keys.First();

        SelectPosition(pos_I_Select);
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

        // 
        for (int i = 0; i < p.Length; i++)
        {
            int who = p[i].who;

            pts[who] += 1;
            txt_Score[who].text = $"{(who == 0 ? "Yellow" : "Blue")}\n{pts[who]}";

            string ppp = "";

            // 
            for (int j = 0; j < p[i].pos.Length; j++)
            {
                peice[p[i].pos[j]].Glow();
                ppp += $"{p[i].pos[j]},";
            }

            print(ppp);
        }

        // 
        if (!game_Active)
        {
            //for (int i = 0; i < p.Length; i++)
            //{
            //    p[i].Print();
            //}

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

    public void Print()
    {
        Debug.Log($"{who} -> {pos[0]} | {pos[1]} | {pos[2]}");
    }
}