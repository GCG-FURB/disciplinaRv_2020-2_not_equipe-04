using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjects : MonoBehaviour
{
    private Player player;
    private GameController gameController;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        this.gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        float distance = this.player.transform.position.z - this.transform.position.z;
        if (distance >= 20f)
        {
            if (this.gameObject.tag == "Boss")
            {
                this.gameController.bossActivated = false;
            }

            Destroy(this.gameObject);
        } 
    }
}
