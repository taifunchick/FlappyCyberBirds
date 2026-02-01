using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;

    public void LoadTheScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
