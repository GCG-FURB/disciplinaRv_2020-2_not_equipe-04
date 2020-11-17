using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonActions : MonoBehaviour
{
    public Image SelectionCircle;
    public float SelectionTimer;
    public CanvasMainMenuControl CanvasMainMenuControl;
    
    
    private float accSelectionTime;
    private Action action;
    private bool isCounterActive;

    private static MenuButtonActions _instance;
    public static MenuButtonActions Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
    }


    void Update()
    {
        if (this.isCounterActive)
        {
            // Debug.Log()
            accSelectionTime += Time.deltaTime;
            if (accSelectionTime < SelectionTimer)
            {
                SelectionCircle.fillAmount = accSelectionTime / SelectionTimer;
            }
            else
            {
                StopCounter();
                action();
            }
        }
    }

    public void StartPlayButtonCounter()
    {
        Debug.Log("aqui");
        this.action = () => CanvasMainMenuControl.PlayButton();
        this.isCounterActive = true;
    }

    public void StartInformationButtonCounter()
    {
        Debug.Log("aqui");
        this.action = () => CanvasMainMenuControl.InformationButton();
        this.isCounterActive = true;
    }

    public void StartRankingButtonCounter()
    {
        Debug.Log("aqui");
        this.action = () => CanvasMainMenuControl.RankingButton();
        this.isCounterActive = true;
    }

    public void StartBackButtonCounter()
    {
        Debug.Log("aqui");
        this.action = () => CanvasMainMenuControl.BackButton();
        this.isCounterActive = true;
    }

    public void StartSaveButtonCounter()
    {
        Debug.Log("aqui");
        this.action = () => CanvasMainMenuControl.SaveButton();
        this.isCounterActive = true;
    }

    public void StartRemakeButtonCounter()
    {
        Debug.Log("aqui");
        this.action = () => CanvasMainMenuControl.RemakeButton();
        this.isCounterActive = true;
    }

    public void StartQuitButtonCounter()
    {
        Debug.Log("aqui");
        this.action = () => CanvasMainMenuControl.QuitButton();
        this.isCounterActive = true;
    }

    public void StopCounter()
    {
        Debug.Log("aqui");
        accSelectionTime = 0f;
        SelectionCircle.fillAmount = 0f;
        isCounterActive = false;
    }
}
