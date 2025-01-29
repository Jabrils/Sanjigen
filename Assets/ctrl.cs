using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Random
// dumdum
// foolish
// cheap
// expensive

public class ctrl : MonoBehaviour
{
    public AudioClip[] sfx_Play;
    public TextMeshProUGUI txt_Win;
    public GameObject end;
    public static ctrl self;
    ctrl_board the_Board;
    public GameObject board;
    AudioSource aS;

    public float rotationSpeed = 100f;
    private Vector3 lastMousePosition;


    void Awake()
    {
        aS = GetComponent<AudioSource>();
        self = this;
        the_Board = FindObjectOfType<ctrl_board>();

        if (Menu.second)
        {
            the_Board.player1 = (ctrl_board.Player)Menu.vsAI;
        }
        else
        {
            the_Board.player2 = (ctrl_board.Player)Menu.vsAI;
        }
    }

    void Start()
    {
        if (!Menu.music)
        {
            aS.Stop();
        }
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

    public void PlayMoveSFX(int w)
    {
        if (Menu.sfx)
        {
            aS.PlayOneShot(sfx_Play[w]);
        }
    }

    public void LoadScene(string scn)
    {
        SceneManager.LoadScene(scn);
    }

    public void GameOver(bool won)
    {
        txt_Win.text = won ? "YOU WIN!" : "YOU LOSE!";

        end.SetActive(true);

        // 
        if (Menu.second)
        {
            PlayerPrefs.SetInt("campaign_ygs", Menu.campaign_ygs + 1);
        }
        else
        {
            PlayerPrefs.SetInt("campaign_ygf", Menu.campaign_ygf + 1);
        }
    }

    public void Fin(int one, int two)
    {
        //print($"{the_Board.player1} = {one}\n{the_Board.player2} = {two}");

        if (Menu.battle_Count.ContainsKey(the_Board.player1.ToString()))
        {
            Menu.battle_Count[the_Board.player1.ToString()].Add(one);
        }
        else
        {
            Menu.battle_Count.Add(the_Board.player1.ToString(), new List<int> { one });
        }

        if (Menu.battle_Count.ContainsKey(the_Board.player2.ToString()))
        {
            Menu.battle_Count[the_Board.player2.ToString()].Add(two);
        }
        else
        {
            Menu.battle_Count.Add(the_Board.player2.ToString(), new List<int> { two });
        }

        // 
        if (Menu.loop == 5)
        {
            print("DONE");

            Menu.PrintAverages();
        }
        else if (Menu.track_2 == 5)
        {
            Menu.loop++;
            Menu.track_1 = 1;
            Menu.track_2 = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        else if (Menu.track_1 == 5)
        {
            Menu.track_1 = 1;
            Menu.track_2++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Menu.track_1++;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
