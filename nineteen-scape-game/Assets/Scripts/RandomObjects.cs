using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjects : MonoBehaviour
{
    private Player player;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        float distance = this.player.transform.position.z - this.transform.position.z;
        if (distance >= 100f) 
        {
            Destroy(this.gameObject);
        } 
    }
}
