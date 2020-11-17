using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMainMenuControl : MonoBehaviour
{
    public Camera Camera;
    public Vector3 CameraDifferenceToMenu;
    public GameObject CanvasMainMenu;
    public GameObject CanvasInformation;
    public GameObject CanvasUsername;
    public GameObject CanvasRanking;
    public CloudScore cloudScore;
    public InputField UsernameInput;
    public List<Text> RankList;

    public VoiceRecognition VoiceRecognition;
    public Button UsernameSaveButton;
    public Button UsernameRemakeButton;

    void Start()
    {
        if (string.IsNullOrWhiteSpace(CloudScore.Username))
        {
            this.VoiceRecognition.recognitionEnabled = true;
            CanvasUsername.SetActive(true);
            CanvasMainMenu.SetActive(false);
        }
        else
        {
            this.VoiceRecognition.recognitionEnabled = false;
            CanvasUsername.SetActive(false);
            CanvasMainMenu.SetActive(true);
        }
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
        
        UsernameInput.text = CloudScore.Username;

        populateRankList();

    }

    void Update()
    {
        this.ActivationNicknameButtons(this.VoiceRecognition.recognized);
        this.updateCameraPosition();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void InformationButton()
    {
        CanvasMainMenu.SetActive(false);
        CanvasUsername.SetActive(false);
        CanvasInformation.SetActive(true);
        CanvasRanking.SetActive(false);
    }

    public void RankingButton()
    {
        CanvasMainMenu.SetActive(false);
        CanvasUsername.SetActive(false);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(true);
    }

    public void BackButton()
    {
        CanvasMainMenu.SetActive(true);
        CanvasUsername.SetActive(false);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
    }

    public void SaveButton()
    {
        CloudScore.Username = UsernameInput.text;
        CanvasMainMenu.SetActive(true);
        CanvasUsername.SetActive(false);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
        this.VoiceRecognition.recognitionEnabled = false;
    }

    public void RemakeButton()
    {
        this.VoiceRecognition.recognized = false;
        UsernameInput.text = "";
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

    private void ActivationNicknameButtons(bool enable)
    {
        this.UsernameSaveButton.interactable = enable;
        this.UsernameRemakeButton.interactable = enable;
    }

    private void updateCameraPosition()
    {
        this.Camera.transform.parent.transform.position = this.transform.position + CameraDifferenceToMenu;
    }

}
