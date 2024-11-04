using UnityEngine;

public class Peice
{
    internal ctrl_board board;
    public Transform obj;
    public Vector3 start, end;
    internal int id;
    bool active;
    Renderer rend;
    float alpha;

    public Peice(ctrl_board board, Transform obj, Vector3 start, Vector3 end)
    {
        this.board = board;
        this.obj = obj;
        this.start = start;
        this.end = end;
        rend = obj.GetComponent<Renderer>();

        //obj.GetComponentInChildren<RCVR_Board_Pos>().peice = this;
        id = int.Parse(obj.name.Split(' ')[1]);
    }

    public void SetToStart()
    {
        obj.localPosition = start;
    }

    public void UnparentBoardPos()
    {
        obj.GetComponentInChildren<RCVR_Board_Pos>().Unparent();
    }

    public void Activate()
    {
        Material[] matty = new Material[]
        {
            new Material(board.this_Turn_Mat),
            new Material(board.mat_Outline),
        };

        rend.materials = matty;

        // 
        if (!active)
        {
            board.AddToBoard(id);
            board.IncrementTurn();
        }

        active = true;
    }

    public void Glow()
    {
        rend.material.SetInt("_Glow", 1);
    }

    public void Update()
    {
        if (active)
        {
            obj.localPosition = Vector3.Lerp(obj.localPosition, end, Time.deltaTime * 4);
            alpha = Mathf.Lerp(alpha, 1, Time.deltaTime * 4);

            // 
            for (int i = 0; i < rend.materials.Length; i++)
            {
                rend.materials[i].SetFloat("_Alpha", alpha);
            }
        }
    }
}