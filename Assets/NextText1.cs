using System;
using Managers;
using player.script;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using weapons.Silk;

public class NextText1: MonoBehaviour
{
    private int totalTag;
    private TextMeshProUGUI cutSceneText;
    private GameObject silk;
    
    private void Start()
    {
        PlayerMove.Instance.gameObject.SetActive(false);
        cutSceneText = GameObject.FindWithTag("timelinetmp").GetComponent<TextMeshProUGUI>();
    }

    private void Check()
    {
        if (totalTag >= 5)
        {
            Destroy(PlayerMove.Instance.gameObject);
            Destroy(SilkThrow.Instance.gameObject);
            SceneManager.LoadScene("Stage2_Rework");
        }

        cutSceneText.text = totalTag switch
        {
            0 => ". . . ?",
            1 => "명줄이 꽤 질기군..",
            2 => "이렇게까지 고전할 줄은 몰랐는데",
            3 => "달의 신이여",
            4 => "무한한 월광의힘을 주소서",
            _ => cutSceneText.text
        };
    }

    public void NextTag()
    {
        totalTag++;
        Check();
    }

    public void ChangeBackgroundMusic()
    {
        AudioManager.SetAsBackgroundMusicInstance("Audio/The_Moonless_Forest", true);
    }
}
