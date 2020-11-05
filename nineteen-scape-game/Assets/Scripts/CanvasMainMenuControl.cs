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

    void Start()
    {
        CanvasMainMenu = GameObject.Find("CanvasMainMenu").gameObject;
        CanvasInformation = GameObject.Find("CanvasInformation").gameObject;
        CanvasRanking = GameObject.Find("CanvasRanking").gameObject;

        CanvasMainMenu.SetActive(true);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
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
        CanvasMainMenu.SetActive(true);
        CanvasInformation.SetActive(false);
        CanvasRanking.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
