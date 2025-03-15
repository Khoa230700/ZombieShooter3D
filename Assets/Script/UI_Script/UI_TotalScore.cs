using UnityEngine;
using UnityEngine.UI;

public class UI_TotalScore : MonoBehaviour
{
    public Text _bodyScoreText;
    public Text _headScoreText;
    public Text _totalScoreText;

    public TotalScore_Script _score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _bodyScoreText.text = _score.GetScoreBody().ToString();
        _headScoreText.text = _score.GetScoreHeadSort().ToString();
        _totalScoreText.text = _score.GetTotalScore().ToString();
    }
}
