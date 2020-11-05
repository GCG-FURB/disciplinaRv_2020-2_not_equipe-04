using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adversaries : MonoBehaviour
{
    public Animator anime;
    private GameController gameController;

    void Start()
    {
        this.transform.Rotate(0f, 180f, 0f);
        this.transform.position += new Vector3(0f, 0.3f, 0f);
        this.gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (!this.gameController.playerDie && !this.gameController.isStopped) 
        {
            this.anime.ResetTrigger("Stop_b");
            this.transform.position += new Vector3(0f, 0f, -0.2f);
        } 
        else
        {
            this.anime.SetTrigger("Stop_b");
        }
    }

    public void Die()
    {
        StartCoroutine(DieRotate());
        this.anime.SetTrigger("Death_b");
        this.disableCollider();
    }

    private void disableCollider()
    {
        Collider[] colliders = this.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders) {
            DestroyImmediate(collider);
        }
    }
    
    IEnumerator DieRotate()
    {
        for (int i = 0; i < 45; i++)
        {
          this.transform.position += new Vector3(-0.025f, 0f, 0f);
          this.transform.Rotate(0f, -1f, 0f);   
          yield return null;  
        }
    }
}
