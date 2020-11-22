using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

public class CloudScore : MonoBehaviour
{

    private static string _username;
    public static string Username { 
        get {
            if (string.IsNullOrWhiteSpace(_username))
            {
                return PlayerPrefs.GetString("username", _username);
            }
            return _username;
        }
        set {
            _username = value;
            PlayerPrefs.SetString("username", _username);
        }
    }

    private static readonly HttpClient client = new HttpClient();
    private static readonly string resource = "https://nineteenscape.firebaseio.com/scores.json";

    private static CloudScore _instance;
    public static CloudScore Instance { get { return _instance; } }

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

    public async void SaveScore(int score)
    {
        if (!string.IsNullOrWhiteSpace(CloudScore.Username))
        {
            GetRanking(scoreList => {
                scoreList.Add(new Score() {
                    username = CloudScore.Username,
                    score = score
                });
                Sort(scoreList);
                scoreList = scoreList.Take(5).ToList();

                string newJsonString = toJsonScoreList(scoreList);

                client.PutAsync(resource, new StringContent(newJsonString, UnicodeEncoding.UTF8, "application/json"));
            });
        }
    }

    public async void GetRanking(Action<List<Score>> rankingAction)
    {
        try
        {
            string jsonString = await client.GetStringAsync(resource);
            rankingAction(toScoreList(jsonString));
        }
        catch (Exception e)
        {
            rankingAction(new List<Score>());
        }
    }

    public async void GetSortedRanking(Action<List<Score>> rankingAction)
    {
        GetRanking(scoreList => {
            Sort(scoreList);
            rankingAction(scoreList);
        });
    }

    private void Sort(List<Score> scoreList)
    {
        scoreList.Sort(delegate(Score o1, Score o2) { return o2.score.CompareTo(o1.score); });
    }

    private List<Score> toScoreList(string json)
    {
        try
        {
            var scoreList = JsonConvert.DeserializeObject<dynamic>(json).ToObject<List<Score>>();
            return scoreList == null ? new List<Score>() : scoreList;
        }
        catch
        {
            throw;
        }
    }

    private string toJsonScoreList(List<Score> scoreList)
    {
        try
        {
            return JsonConvert.SerializeObject(scoreList);
        }
        catch
        {
            throw;
        }
    }

    [DataContract]
    public class Score
    {
        [DataMember(Name = "username")]
        public string username { get; set; }
        [DataMember(Name = "score")]
        public int score { get; set; }
    }

}
