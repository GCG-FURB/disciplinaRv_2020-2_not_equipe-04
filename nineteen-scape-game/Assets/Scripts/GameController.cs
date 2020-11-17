using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics;

public class GameController : MonoBehaviour
{
    public float score;
    public float speedIncreaseRate;
    public float horizontalSpeedIncreaseRate;
    public int bossDisplayRate;

    public Alert alert;
    public Canvas CanvasInteractive;
    public List<GameObject> randomObjects = new List<GameObject>();
    public List<int> randomObjectsProbabilities = new List<int>();
    public GameObject gameOver;
    public Text scoreText;
    public Text gameOverScoreText;
    public bool playerDie = false;
    public bool isStopped = false;
    public bool bossActivated;
    public BossController boss;

    private CloudScore cloudScore;
    private Player player;
    private float time;

    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
    }

    void Start()
    {
        this.cloudScore = FindObjectOfType<CloudScore>();
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        this.validateRandomObjectsProbabilities();
    }

    void Update()
    {
        if(!this.playerDie && !this.isStopped)
        {
            if (player.Infected)
            {
                this.player.jumpHeight = 0f;
                if (player.speed > 0)
                {
                    this.player.speed -= this.speedIncreaseRate * 5f;
                    this.player.horizontalSpeed -= this.horizontalSpeedIncreaseRate * 5f;
                    this.alert.Show("AlterChloroquine");
                }
            }
            else
            {
                this.player.speed += this.speedIncreaseRate;
                this.player.horizontalSpeed += this.horizontalSpeedIncreaseRate;
                this.score += Time.deltaTime * 5f;
                this.scoreText.text = Mathf.Round(this.score).ToString() + "m";
            }

            if (!bossActivated)
            {
                if (score > 1f && Mathf.Round(score) % bossDisplayRate == 0)
                {
                    this.initializeBoss();
                } 
                else
                {
                    this.createRandomObjectInScene();
                }
            }
        }
    }

    public void CallGameOver()
    {
        this.cloudScore.SaveScore((int)Mathf.Round(this.score));
        this.gameOverScoreText.text = string.Format("Atingido {0}m", Mathf.Round(this.score));
        this.RepositionCanvas();
        this.gameOver.SetActive(true);
    }

    private void createRandomObjectInScene()
    {
        if(this.time > 10)
        {
            GameObject newObject = this.getRandomObject();
            Instantiate(newObject, this.getRandomVector3(Random.Range(1, 4), newObject.transform.position.y), newObject.transform.rotation);
            this.time = 0f;
        }
        else
        {
            this.time += Time.deltaTime * 3f + (this.player.speed / 100);
        }
    }

    public void RepositionCanvas()
    {
        this.CanvasInteractive.transform.position = this.player.transform.position + new Vector3(0f, 0f, 1.3f);
    }

    private Vector3 getRandomVector3(int position, float y)
    {
        float x = -5;
        float z = this.player.transform.position.z + 120f;

        if (position == 2) 
        {
            x = 0;
        }
        else if (position == 3) 
        {
            x = 5;
        }

        return new Vector3(x, y, z);
    }

    private GameObject getRandomObject()
    {
        int random = Random.Range(0, 100);
        int accumulator = 0;
        for (int i = 0; i < this.randomObjects.Count; i++)
        {
            int probability = this.randomObjectsProbabilities[i];
            GameObject gameObject = this.randomObjects[i];
            
            accumulator += probability;
            if (random < accumulator)
            {
                return gameObject;
            }
        }
        throw new System.ArgumentException("Cannot pick random object");
    }

    private void validateRandomObjectsProbabilities()
    {
        if (this.randomObjectsProbabilities.Count != this.randomObjects.Count) 
        {
            throw new System.ArgumentException("Random Objects Probabilities should have the same number of elements as Random Object");
        }
        if (this.randomObjectsProbabilities.Sum() != 100)
        {
            throw new System.ArgumentException("Random Objects Probabilities should have sum equal to 100");
        }
    }

    private void initializeBoss()
    {
        this.alert.Show("CallBoss");
        this.bossActivated = true;
        Invoke("CallBoss", 2f);
    }

    private void CallBoss()
    {
        var x = this.player.transform.position.x;
        var y = this.boss.transform.position.y;
        var z = this.player.transform.position.z + 120f;
        Instantiate(this.boss, new Vector3(x, y, z), this.boss.transform.rotation);
    }
}
