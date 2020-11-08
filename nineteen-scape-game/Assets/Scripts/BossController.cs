using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class BossController : MonoBehaviour
{
    public Animator anime;
    private GameController gameController;
    private Player player;
    private bool isMoving;

    void Start()
    {
        this.transform.position = MotionUtility.Normalize(this.gameObject);
        this.transform.Rotate(0f, 180f, 0f);
        this.gameController = FindObjectOfType<GameController>();
        this.player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (!this.gameController.playerDie && !this.gameController.isStopped)
        {
            this.anime.ResetTrigger("Stop_b");
            this.transform.position += new Vector3(0f, 0f, -0.2f);
            
            if (Mathf.Round(this.transform.position.z - this.player.transform.position.z) % 50 == 0)
            {
                this.RandomMovement();
            }
        }
        else
        {
            this.anime.SetTrigger("Stop_b");
        }
    }

    private void RandomMovement()
    {
        var action = UnityEngine.Random.Range(0, 2);

        if (!isMoving)
        {
            if (action == 0)
            {
                StartCoroutine(Move(this.transform.position.x, x => x > -5, x => this.transform.position.x >= x - 5f, -0.1f));
            }
            else if (action == 1)
            {
                StartCoroutine(Move(this.transform.position.x, x => x < 5, x => this.transform.position.x <= x + 5f, 0.1f));
            }
        }

    }

    IEnumerator Move(float x, Func<float, bool> isValid, Func<float, bool> predicate, float rate)
    {
        isMoving = true;

        if (isValid(x))
        {
            while (predicate(x))
            {
                this.transform.position += new Vector3(rate, 0f, 0f);
                yield return null;
            }
        }

        isMoving = false;
        this.transform.position = MotionUtility.Normalize(this.gameObject);
    }
}
