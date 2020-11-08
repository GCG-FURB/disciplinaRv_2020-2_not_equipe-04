using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Runtime.Serialization;

public class CloudScore : MonoBehaviour
{

    public static string Username { get; set; }

    private static readonly HttpClient client = new HttpClient();
    private static readonly string resource = "https://nineteenscape.firebaseio.com/scores.json";

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
        string jsonString = await client.GetStringAsync(resource);
        rankingAction(toScoreList(jsonString));
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
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Score>));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            List<Score> scoreList = (List<Score>)serializer.ReadObject(ms);
            ms.Close();
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
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Score>));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, scoreList);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
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
