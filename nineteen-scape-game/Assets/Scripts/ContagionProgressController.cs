using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ContagionProgressController : MonoBehaviour
{

    public string InfectionFreeMessage;
    public float InfectionFreeMessageTime; // Em segundos
    public float RandomizationScoreInterval; // Em metros
    
    private Alert alert;
    private RectTransform backgroundProgressRectTransform;
    private int currentPercentage;
    private RectTransform overlayProgressRectTransform;
    private BuffController buffController;
    private GameController gameController;
    private Text percentageContagionProgress;

    private float accDeltaInfectionFreeMessage;
    private bool isAnimatingBar;
    private bool showingMessage;
    
    void Start()
    {
        backgroundProgressRectTransform = GameObject.FindGameObjectWithTag("BackgroundContagionProgress").GetComponent<RectTransform>();
        overlayProgressRectTransform = GameObject.FindGameObjectWithTag("OverlayContagionProgress").GetComponent<RectTransform>();
        percentageContagionProgress = GameObject.FindGameObjectWithTag("PercentageContagionProgress").GetComponent<Text>();

        alert = FindObjectOfType<Alert>();
        gameController = FindObjectOfType<GameController>();
        buffController = FindObjectOfType<BuffController>();
        
        accDeltaInfectionFreeMessage = 0f;
        isAnimatingBar = false;
        showingMessage = false;
    }

    void Update()
    {
        if (!gameController.bossActivated)
        {
            if (Mathf.Round(gameController.score) % RandomizationScoreInterval == 0 && !gameController.playerDie && !buffController.IsWaitingForPillAction && !isAnimatingBar)
            {
                var oldPercentage = currentPercentage;
                currentPercentage = random(0, 100);

                if (oldPercentage < currentPercentage)
                {
                    StartCoroutine(AnimateContagionBar(oldPercentage, i => i < currentPercentage, i => i + 1));
                }
                else
                {
                    StartCoroutine(AnimateContagionBar(oldPercentage, i => i > currentPercentage, i => i - 1));
                }

            }

            if (showingMessage)
            {
                accDeltaInfectionFreeMessage += Time.deltaTime;
                if (accDeltaInfectionFreeMessage >= InfectionFreeMessageTime)
                {
                    showingMessage = false;
                    accDeltaInfectionFreeMessage = 0f;
                    percentageContagionProgress.text = currentPercentage.ToString();
                }
            }
        }
    }

    IEnumerator AnimateContagionBar(float start, Func<float, bool> predicate, Func<float, float> incFunction)
    {
        isAnimatingBar = true;    
        var height = overlayProgressRectTransform.rect.height;

        for (var i = start; predicate(i); i = incFunction(i))
        {
            var width = backgroundProgressRectTransform.rect.width / 100 * i;

            overlayProgressRectTransform.sizeDelta = new Vector2(width, height);

            var x = (backgroundProgressRectTransform.rect.width / 2) - width / 2; // Alinhado a direita
            var y = backgroundProgressRectTransform.localPosition[1];
            var z = backgroundProgressRectTransform.localPosition[2];
            
            overlayProgressRectTransform.localPosition = new Vector3(x, y, z);

            if (!showingMessage)
            {
                percentageContagionProgress.text = i.ToString();
            }

            yield return null;
        }
        isAnimatingBar = false;
    }

    public bool RandomizedInfection()
    {
        var result = random(0, 100) <= this.currentPercentage;
        if (!result)
        {
            showingMessage = true;
            // percentageContagionProgress.text = InfectionFreeMessage;
            alert.Show("AdversersaryWithoutCovid");
        }
        return result;
    }

    private int random(int lower, int higher)
    {
        return UnityEngine.Random.Range(lower, higher);
    }
    

}
