using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Score : MonoBehaviour
{
    public static Score _instance;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    private int _score;
    private readonly List<TextMeshProUGUI> _currentScoreTexts = new List<TextMeshProUGUI>();
    private readonly List<TextMeshProUGUI> _highScoreTexts = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            RegisterTexts(_currentScoreText, _highScoreText);
        }
        else if (_instance != this)
        {
            _instance.RegisterTexts(_currentScoreText, _highScoreText);
            enabled = false;
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        UpdateScoreTexts();
        UpdateHighScoreTexts(PlayerPrefs.GetInt("HighScore", 0));
        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if (_score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", _score);
            UpdateHighScoreTexts(_score);
        }
    }

    public void UpdateScore()
    {
        _score++;
        UpdateScoreTexts();
        UpdateHighScore();
    }

    private void RegisterTexts(TextMeshProUGUI currentScoreText, TextMeshProUGUI highScoreText)
    {
        if (currentScoreText != null && !_currentScoreTexts.Contains(currentScoreText))
        {
            _currentScoreTexts.Add(currentScoreText);
        }

        if (highScoreText != null && !_highScoreTexts.Contains(highScoreText))
        {
            _highScoreTexts.Add(highScoreText);
        }
    }

    private void UpdateScoreTexts()
    {
        string scoreValue = _score.ToString();
        foreach (TextMeshProUGUI text in _currentScoreTexts)
        {
            if (text != null)
            {
                text.text = scoreValue;
            }
        }
    }

    private void UpdateHighScoreTexts(int value)
    {
        string highScoreValue = value.ToString();
        foreach (TextMeshProUGUI text in _highScoreTexts)
        {
            if (text != null)
            {
                text.text = highScoreValue;
            }
        }
    }
}
