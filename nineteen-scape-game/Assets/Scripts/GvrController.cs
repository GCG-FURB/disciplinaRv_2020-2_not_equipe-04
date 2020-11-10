using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GvrController : MonoBehaviour
{
    public Player player;
    public Camera camera;
    public float horizontalMovementRate;
    public float verticalMovementRate;

    void Update()
    {
        if (!this.player.isMoving)
        { 
            if (this.camera.transform.localRotation.y <= -horizontalMovementRate) 
            {
                this.player.left = true;
            } 
            else if (this.camera.transform.localRotation.y >= horizontalMovementRate) 
            {
                this.player.right = true;
            }
            else if (this.camera.transform.localRotation.x <= -verticalMovementRate)
            {
                this.player.jump = true;
            }
        }
    }
}
