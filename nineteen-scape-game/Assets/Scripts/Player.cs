using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public const float playerLimit = 3f;
    public float speed;
    public float horizontalSpeed;
    public float jumpHeight;
    public float gravity;
    public float rayRadius;
    public LayerMask adversariesLayer;
    public LayerMask bossLayer;
    public Animator anime;
    public bool Infected;

    private ContagionProgressController contagionProgressController;
    private CharacterController controller;
    private float jumpVelocity;
    private bool isMoving;
    private GameController gameController;
    private BuffController buffController;

    private float currentSpeed;
    private float currentJumpHeight;
    private float currentHorizontalSpeed;

    void Start()
    {
        this.contagionProgressController = FindObjectOfType<ContagionProgressController>();
        this.controller = GetComponent<CharacterController>();
        this.gameController = FindObjectOfType<GameController>();
        this.buffController = FindObjectOfType<BuffController>();
    }

    void Update()
    {
        if (!this.gameController.isStopped) 
        {
            this.anime.ResetTrigger("Stop_b");
            Vector3 direction = Vector3.forward * speed;   
            
            if(this.controller.isGrounded) 
            {
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    this.jumpVelocity = this.jumpHeight;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow) && this.transform.position.x < playerLimit && !this.isMoving)
                {
                    this.isMoving = true;
                    StartCoroutine(RightMove());
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) && this.transform.position.x > -playerLimit && !this.isMoving)
                {
                    this.isMoving = true;
                    StartCoroutine(LeftMove());
                }
            }
            else
            {
                this.jumpVelocity -= this.gravity;
            }

            this.OnCollision();
            direction.y = this.jumpVelocity;
            this.controller.Move(direction * Time.deltaTime);
        } 
        else
        {
            this.anime.SetTrigger("Stop_b");
        }
    }

    IEnumerator LeftMove()
    {
        float x = this.transform.position.x;

        if (x > -5) 
        {
            while (this.transform.position.x >= x - 5f) 
            {
                this.controller.Move(Vector3.left * Time.deltaTime * this.horizontalSpeed);
                yield return null;
            }
        }        
        this.normalize();
        this.isMoving = false;        
    }

    IEnumerator RightMove()
    {
        float x = this.transform.position.x;

        if (x < 5) 
        {
            while (this.transform.position.x <= x + 5f) 
            {
                this.controller.Move(Vector3.right * Time.deltaTime * this.horizontalSpeed);
                yield return null;
            }
        }     
        this.normalize();
        this.isMoving = false;
    }

    private void normalize()
    {
        float x = this.transform.position.x;
        float currentX;

        if (x < -3f) 
        {
            currentX = -5f;
        } 
        else if (x > 3f) 
        {
            currentX = 5f;
        }
        else 
        {
            currentX = 0f;
        }

        this.transform.position = new Vector3(currentX, this.transform.position.y, this.transform.position.z);
    }

    void OnCollision()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit, this.rayRadius, this.adversariesLayer) && !this.gameController.playerDie)
        {
            hit.collider.gameObject.GetComponent<Adversaries>().Die();
            if (!this.Infected && this.contagionProgressController.RandomizedInfection())
            {
                this.ValidatePlayerBuff();
            }
        }

        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit, this.rayRadius, this.bossLayer) && !this.gameController.playerDie)
        {
            this.StopRunning();
            this.Die2();
        }
    }

    private void ValidatePlayerBuff()
    {
        int action = this.buffController.Action();
        if (action != 0) 
        {
            this.StopRunning();
            if (action == -1) 
            {
                this.Die();
            }
        }

    }

    public void Die()
    {
        this.gameController.playerDie = true;
        this.anime.SetTrigger("Death_player_b");
        this.anime.SetTrigger("Death_b");
        Invoke("GameOverAlert", 3f);
    }

    public void Die2()
    {
        this.gameController.playerDie = true;
        this.anime.ResetTrigger("Death_player_b");
        this.anime.SetTrigger("Death_b");
        Invoke("GameOverAlert", 3f);
    }

    public void Revive()
    {
        this.gameController.playerDie = false;
        this.gameController.isStopped = false;
        this.speed = this.currentSpeed;
        this.horizontalSpeed = this.currentHorizontalSpeed;
        this.jumpHeight = this.currentJumpHeight;
    }

    private void GameOverAlert()
    {
        this.gameController.CallGameOver();
    }

    public void StopRunning() 
    {
        this.gameController.isStopped = true;
        this.currentSpeed = this.speed;
        this.currentHorizontalSpeed = this.horizontalSpeed;
        this.currentJumpHeight = this.jumpHeight;
        this.speed = 0;
        this.jumpHeight = 0;
        this.horizontalSpeed = 0;
    }
}
