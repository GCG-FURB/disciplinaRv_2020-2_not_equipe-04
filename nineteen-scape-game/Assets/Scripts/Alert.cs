using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public GameController gameController;
    public List<string> ImageTags;
    public Dictionary<string, RawImage> imageDict;
    public int ShownTime;
    
    private bool isExecuting;

    void Start()
    {
        this.imageDict = new Dictionary<string, RawImage>();
        foreach (var imageTag in this.ImageTags)
        {
            this.imageDict.Add(imageTag, GameObject.FindGameObjectWithTag(imageTag).GetComponent<RawImage>());
        }
        this.isExecuting = false;
    }

    public void Show(string key)
    {
        if (!isExecuting)
        {
            StartCoroutine(FadeIn(key));
        }
    }

    IEnumerator FadeIn(string key) 
    {
        isExecuting = true;
        
        var rawImage = imageDict[key];
        
        var r = rawImage.color.r;
        var g = rawImage.color.g;
        var b = rawImage.color.b;

        for (float i = 0; (i <= 1 && shouldContinue()); i += Time.deltaTime)
        {
            rawImage.color = new Color(r, g, b, i);
            yield return null;
        }

        yield return new WaitForSeconds(ShownTime);

        for (float i = 1; (i >= 0 && shouldContinue()); i -= Time.deltaTime)
        {
            rawImage.color = new Color(r, g, b, i);
            yield return null;
        }

        rawImage.color = new Color(r, g, b, 0);

        isExecuting = false;
    }

    private bool shouldContinue()
    {
        return !this.gameController.playerDie && !this.gameController.isStopped;
    }

}
