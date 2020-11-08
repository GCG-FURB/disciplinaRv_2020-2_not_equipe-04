using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMainMenuControl : MonoBehaviour
{
    private GameObject CanvasMainMenu;
    private GameObject CanvasInformation;
    private GameObject CanvasRanking;

    private CloudScore cloudScore;

    public Text UsernameInput;
    public List<Text> RankList;

    void Start()
    {
        CanvasMainMenu = GameObject.Find("CanvasMainMenu").gameObject;
        CanvasInformation = GameObject.Find("CanvasInformation").gameObject;
        CanvasRanking = GameObject.Find("CanvasRanking").gameObject;
        
        cloudScore = FindObjectOfType<CloudScore>();

        CanvasMainMenu.SetActive(true);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
        UsernameInput.text = CloudScore.Username;

        populateRankList();

    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void InformationButton()
    {
        CanvasMainMenu.SetActive(false);
        CanvasInformation.SetActive(true);
        CanvasRanking.SetActive(false);
    }

    public void RankingButton()
    {
        CanvasMainMenu.SetActive(false);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(true);
    }

    public void BackButton()
    {
        CanvasMainMenu.SetActive(true);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
    }

    public void SaveButton()
    {
        CloudScore.Username = UsernameInput.text;
        CanvasMainMenu.SetActive(true);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void populateRankList()
    {
        cloudScore.GetRanking(ranking => {
            for (var i = 0; i < ranking.Count; i++)
            {
                var score = ranking[i];
                var text = RankList[i];

                text.text = "0" + (i + 1) + " - " + score.username + " - " + score.score;
            }
            for (var i = ranking.Count; i < RankList.Count; i++)
            {
                RankList[i].text = "";
            }
        });
    }

}
