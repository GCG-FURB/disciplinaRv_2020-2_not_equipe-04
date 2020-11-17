using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtonActions : MonoBehaviour
{
    public BuffController BuffController;
    public GameController GameController;
    public Image SelectionCircle;
    public float SelectionTimer;
    
    
    private float accSelectionTime;
    private Action action;
    private bool isCounterActive;

    private static GameButtonActions _instance;
    public static GameButtonActions Instance { get { return _instance; } }
    
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
            GameController.RepositionCanvas();
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

    public void returnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void acceptChloroquine()
    {
        this.BuffController.CurrentPillAction = 1;
    }

    public void refuseChloroquine()
    {
        this.BuffController.CurrentPillAction = 2;
    }

    public void StartReturnToMenuCounter()
    {
        this.action = () => SceneManager.LoadScene("MenuScene");
        this.isCounterActive = true;
    }

    public void StartPillActionCounter(int pillAction)
    {
        this.action = () => BuffController.CurrentPillAction = pillAction;
        this.isCounterActive = true;
    }

    public void StopCounter()
    {
        accSelectionTime = 0f;
        SelectionCircle.fillAmount = 0f;
        isCounterActive = false;
    }
}
