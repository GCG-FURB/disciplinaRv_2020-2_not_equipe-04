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
        this.action = () => CanvasMainMenuControl.PlayButton();
        this.isCounterActive = true;
    }

    public void StartInformationButtonCounter()
    {
        this.action = () => CanvasMainMenuControl.InformationButton();
        this.isCounterActive = true;
    }

    public void StartRankingButtonCounter()
    {
        this.action = () => CanvasMainMenuControl.RankingButton();
        this.isCounterActive = true;
    }

    public void StartBackButtonCounter()
    {
        this.action = () => CanvasMainMenuControl.BackButton();
        this.isCounterActive = true;
    }

    public void StartSaveButtonCounter()
    {
        this.action = () => CanvasMainMenuControl.SaveButton();
        this.isCounterActive = true;
    }

    public void StartRemakeButtonCounter()
    {
        this.action = () => CanvasMainMenuControl.RemakeButton();
        this.isCounterActive = true;
    }

    public void StartQuitButtonCounter()
    {
        this.action = () => CanvasMainMenuControl.QuitButton();
        this.isCounterActive = true;
    }

    public void StopCounter()
    {
        accSelectionTime = 0f;
        SelectionCircle.fillAmount = 0f;
        isCounterActive = false;
    }
}
