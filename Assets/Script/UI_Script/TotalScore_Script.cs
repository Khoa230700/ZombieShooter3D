using UnityEngine;
using UnityEngine.UI;

public class TotalScore_Script : MonoBehaviour
{

    [SerializeField] private float _scoreBody ;
    [SerializeField] private float _scoreHead ;
    [SerializeField] private float _totalScore;

    void Start()
    {
        _scoreBody = 0;
        _scoreHead= 0;
        _totalScore = 0;
    }

    public float GetScoreBody() => _scoreBody;
    public float GetScoreHeadSort() => _scoreHead;

    public float GetTotalScore() => _totalScore = _scoreBody + _scoreHead;

    public void ScoreHeadSort(float health)
    {
       _scoreHead += health;
    }
    public void ScoreBody(float health)
    {
        _scoreBody += health;
    }
  
}
