using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public RawImage Image;
    public Text SuccessfulAlertUseMask;

    public void AnimateMask(bool fadeIn)
    {
        if (fadeIn) 
        {
            StartCoroutine(FadeIn(SuccessfulAlertUseMask));    
        }
        else
        {
            StartCoroutine(FadeOut(SuccessfulAlertUseMask));    
        }   
    }

    IEnumerator FadeIn(Text textAnimate) 
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            this.Image.color = new Color(this.Image.color.r, this.Image.color.g, this.Image.color.b, i);
            textAnimate.color = new Color(textAnimate.color.r, textAnimate.color.g, textAnimate.color.b, i);
            yield return null;
        }
    }

    IEnumerator FadeOut(Text textAnimate) 
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            this.Image.color = new Color(this.Image.color.r, this.Image.color.g, this.Image.color.b, i);
            textAnimate.color = new Color(textAnimate.color.r, textAnimate.color.g, textAnimate.color.b, i);
            yield return null;
        }
    }
}
