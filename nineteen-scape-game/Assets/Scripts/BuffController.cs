using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuffController : MonoBehaviour
{
    class Buff
    {
        private Text ui;
        private int value = 0;

        public Buff(Text ui) {
            this.ui = ui;
        }

        public int getValue()
        {
            return this.value;
        }

        public void Increment() 
        {
            this.value++;
            this.PrintValue();
        }

        public void Decrement() 
        {
            this.value--;
            this.PrintValue();
        }

        private void PrintValue()
        {
            this.ui.text = this.value.ToString();
        }
    }
    
    /* game objects to control ui texts */
    public GameController GameController;
    public Text PillUI;
    public GameObject PillAlert;
    public Text MaskUI;
    public Player Player;
    public Alert alert;

    /* variable of interaction with the buttons */
    public int CurrentPillAction = 0;
    public bool IsWaitingForPillAction = false;

    public enum E_BUFF {Mask, Pill};
    private Dictionary<string, Buff> buffDictionary = new Dictionary<string, Buff>();

    private static BuffController _instance;
    public static BuffController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
    }

    void Start()
    {
        this.StartValuesBuffs();
    }

    public void Increment(E_BUFF type, GameObject buffObject)
    {        
        this.buffDictionary[type.ToString()].Increment();
        Destroy(buffObject);
    }

    public void Decrement(E_BUFF type)
    {
        this.buffDictionary[type.ToString()].Decrement();
    }

    public int Action() 
    {
        foreach(E_BUFF buff in Enum.GetValues(typeof(E_BUFF)))
        {
            if (this.buffDictionary[buff.ToString()].getValue() > 0) 
            {
                return this.Action(buff);
            }
        }

        return -1;
    } 

    private int Action(E_BUFF type)
    {
        switch (type)
        {
            case E_BUFF.Mask:
                this.alert.Show("AlterMaskAntiCovid");
                this.Decrement(type);
                return 0;

            case E_BUFF.Pill:
                this.PillAlert.SetActive(true);
                GameController.RepositionCanvas();
                StartCoroutine(WaitForPillAction());
                return 1;

            default:
                throw new System.ArgumentException("Invalid buff");
        }
    }

    private void StartValuesBuffs()
    {
        this.buffDictionary.Add(E_BUFF.Pill.ToString(), new Buff(PillUI));
        this.buffDictionary.Add(E_BUFF.Mask.ToString(), new Buff(MaskUI));

        if (this.buffDictionary.Count != Enum.GetValues(typeof(E_BUFF)).Length) {
            throw new System.ArgumentException("Number of buffs specified different from the number of instantiated");
        }
    }

    IEnumerator WaitForPillAction()
    {
        IsWaitingForPillAction = true;
        while(this.CurrentPillAction == 0)
        {
            yield return null;
        }
        
        this.PillAlert.SetActive(false);

        if (this.CurrentPillAction == 1) {
            this.RandomPillAction();
        } else {
            this.Player.Die();
        }        

        this.CurrentPillAction = 0;
        IsWaitingForPillAction = false;
    }

    private void RandomPillAction()
    {
        this.Decrement(E_BUFF.Pill);
        this.Player.Revive();
        
        int isHealed = UnityEngine.Random.Range(0, 2);
        if (isHealed == 0) 
        {
            this.Player.Infected = true;
            StartCoroutine(PillFailure());
        }
        else
        {
            this.alert.Show("SaveByChloroquine");
        }
    }

    IEnumerator PillFailure()
    {
        int i = 10;
        while(i > 5)
        {
            i--;
            yield return new WaitForSeconds(1);
        }

        this.Player.StopRunning();
        this.Player.Die();
    }
}
