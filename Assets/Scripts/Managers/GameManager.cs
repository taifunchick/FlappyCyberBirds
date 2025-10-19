using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [SerializeField] private GameObject _gameOverCanvas;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        _gameOverCanvas.SetActive(true);

        Time.timeScale = 0;

        YG2.InterstitialAdvShow();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
