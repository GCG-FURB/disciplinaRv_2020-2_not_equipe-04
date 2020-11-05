using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtonActions : MonoBehaviour
{
    public BuffController BuffController;

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

    public void returnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void acceptChloroquine()
    {
        this.BuffController.CurrentPillAction = 1;
    }

    public void refuseChloroquine()
    {
        this.BuffController.CurrentPillAction = 2;
    }
}
