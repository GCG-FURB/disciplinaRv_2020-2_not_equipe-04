using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskCollider : MonoBehaviour
{
    private BuffController buffController;

    void Start()
    {
        this.buffController = FindObjectOfType<BuffController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        this.buffController.Increment(BuffController.E_BUFF.Mask, this.gameObject);     
    }
}
