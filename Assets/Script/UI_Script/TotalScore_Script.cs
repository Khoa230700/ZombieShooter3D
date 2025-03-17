using UnityEngine;
using UnityEngine.UI;

public class TotalScore_Script : MonoBehaviour
{

    [SerializeField] public float _scoreBody ;
    [SerializeField] public float _scoreHead ;
    [SerializeField] public float _totalScore;
    public float MissionPoint;
    void Start()
    {
        _scoreBody = 0;
        _scoreHead= 0;
        _totalScore = 0;
        MissionPoint = 0;
    }

    public float GetScoreBody() => _scoreBody;
    public float GetScoreHeadSort() => _scoreHead;

    public float GetTotalScore() => _totalScore = (_scoreBody + _scoreHead*2)*10 + MissionPoint;

    public void ScoreHeadSort(float health)
    {
       _scoreHead += health;
    }
    public void ScoreBody(float health)
    {
        _scoreBody += health;
    }
  
}
