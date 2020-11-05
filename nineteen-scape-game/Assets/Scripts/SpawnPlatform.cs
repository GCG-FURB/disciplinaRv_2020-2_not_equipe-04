using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    public const int platformSize = 86;

    public List<GameObject> platforms = new List<GameObject>();
    public List<Transform> currentPlatforms = new List<Transform>();

    public int offset;
    private Transform player;
    private Transform currentPlatformPoint;
    private int platformIndex;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;

        for(int i = 0; i < this.platforms.Count; i++)
        {
            Transform platform = Instantiate(this.platforms[i], new Vector3(0, 0, i * platformSize), this.transform.rotation).transform;
            this.currentPlatforms.Add(platform);
            this.offset += 86;
        }

        this.currentPlatformPoint = this.currentPlatforms[this.platformIndex].GetComponent<Platform>().point;
    }

    void Update()
    {
        float distance = this.player.position.z - this.currentPlatformPoint.position.z;
        if (distance >= 5)
        {
            this.Recycle(this.currentPlatforms[this.platformIndex].gameObject);
            this.platformIndex++;

            if(this.platformIndex > this.currentPlatforms.Count -1) 
            {
                this.platformIndex = 0;
            }

            this.currentPlatformPoint = this.currentPlatforms[this.platformIndex].GetComponent<Platform>().point;
        }
    }

    public void Recycle(GameObject platform)
    {
        platform.transform.position = new Vector3(0, 0, this.offset);
        this.offset += platformSize;
    }
}
