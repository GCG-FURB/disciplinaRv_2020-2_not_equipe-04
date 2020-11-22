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
        GameController.RepositionCanvas();
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
        Debug.Log("Entrou");
        this.action = () => SceneManager.LoadScene("MenuScene");
        this.isCounterActive = true;
    }

    public void StartPillActionCounter(int pillAction)
    {
        Debug.Log("Entrou");
        this.action = () => BuffController.CurrentPillAction = pillAction;
        this.isCounterActive = true;
    }

    public void StopCounter()
    {
        Debug.Log("Saiu");
        accSelectionTime = 0f;
        SelectionCircle.fillAmount = 0f;
        isCounterActive = false;
    }
}
