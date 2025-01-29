using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public AudioClip select;
    public Camera cam;
    public menu_mover[] mm;
    AudioSource aS;
    public Button btn_ygs;
    public CanvasGroup cg_ygs, cg_wipe;
    public Button[] btn_sel;
    public CanvasGroup[] cg_sel;
    public TextMeshProUGUI txt_Music, txt_SFX;

    public static int track_1 = 1;
    public static int track_2 = 1;
    public static Dictionary<string, List<int>> battle_Count = new Dictionary<string, List<int>>();
    public static int loop;

    public static bool second;
    public static int vsAI;
    public static int campaign_ygf;
    public static int campaign_ygs;
    public static bool music, sfx;
    float cam_end_move = 2f;

    private void Awake()
    {
        // 
        if (PlayerPrefs.HasKey("music"))
        {
            music = PlayerPrefs.GetInt("music") == 0 ? false : true;
        }
        else
        {
            PlayerPrefs.SetInt("music", 1);
            music = true;
        }

        // 
        if (PlayerPrefs.HasKey("sfx"))
        {
            sfx = PlayerPrefs.GetInt("sfx") == 0 ? false : true;
        }
        else
        {
            PlayerPrefs.SetInt("sfx", 1);
            sfx = true;
        }

        campaign_ygf = PlayerPrefs.GetInt("campaign_ygf");
        campaign_ygs = PlayerPrefs.GetInt("campaign_ygs");

        print(campaign_ygf);
    }

    private void Start()
    {
        aS = GetComponent<AudioSource>();

        if (!music)
        {
            aS.Stop();
        }

        btn_ygs.interactable = campaign_ygf >= 5;
        cg_ygs.alpha = campaign_ygf >= 5 ? 1 : .5f;

        txt_Music.text = music ? "Disable Music" : "Enable Music";
        txt_SFX.text = sfx ? "Disable SFX" : "Enable SFX";

    }
    private void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(cam_end_move, 0, 1.94f), Time.deltaTime);
        cg_wipe.alpha -= Time.deltaTime * .2f;
    }

    public void ActivateMainMenu()
    {
        PlaySelectSFX();
        for (int i = 0; i < mm.Length; i++)
        {
            mm[i].active = false;
        }

        mm[0].active = true;

    }

    private void PlaySelectSFX()
    {
        if (sfx)
        {
            aS.PlayOneShot(select);
        }
    }

    public void ActivateCampMenu()
    {
        PlaySelectSFX();
        for (int i = 0; i < mm.Length; i++)
        {
            mm[i].active = false;
        }

        mm[1].active = true;
    }
    public void ActivateSelectMenu()
    {
        for (int i = 0; i < btn_sel.Length; i++)
        {
            btn_sel[i].interactable = second ? campaign_ygs >= i : campaign_ygf >= i;
            cg_sel[i].alpha = second ? (campaign_ygs >= i ? 1 : .5f) : (campaign_ygf >= i ? 1 : .5f);
        }

        for (int i = 0; i < mm.Length; i++)
        {
            mm[i].active = false;
        }

        mm[2].active = true;
    }
    public void ActivateSettingsMenu()
    {
        PlaySelectSFX();
        for (int i = 0; i < mm.Length; i++)
        {
            mm[i].active = false;
        }

        mm[3].active = true;
    }

    public void ResetCampaignData()
    {
        PlaySelectSFX();
        PlayerPrefs.SetInt("campaign_ygf", 0);
        PlayerPrefs.SetInt("campaign_ygs", 0);
        cg_wipe.alpha = 1;
    }
    public void ToggleMusic()
    {
        PlaySelectSFX();
        music = music ? false : true;
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        txt_Music.text = music ? "Disable Music" : "Enable Music";

        // 
        if (music)
        {
            aS.Play();
        }
        else
        {
            aS.Stop();
        }
    }

    public void ToggleSFX()
    {
        PlaySelectSFX();
        sfx = sfx ? false : true;
        PlayerPrefs.SetInt("sfx", sfx ? 1 : 0);
        txt_SFX.text = sfx ? "Disable SFX" : "Enable SFX";
    }

    public void Select2nd(bool n)
    {
        PlaySelectSFX();
        second = n;

        ActivateSelectMenu();
    }
    public void SelectAI(int ai)
    {
        PlaySelectSFX();
        vsAI = ai;

        GoToScene("Game");
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void PrintAverages()
    {
        foreach (var kvp in battle_Count)
        {
            string key = kvp.Key;
            List<int> values = kvp.Value;

            double average = values.Any() ? values.Average() : 0;  // Check to avoid division by zero
            print($"{key}: {average:F2}");
        }
    }
}
