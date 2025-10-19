using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using YG;

public class AsyncLoader : MonoBehaviour
{
    public Slider progressBar;                  // Ссылка на UI Slider
    public TextMeshProUGUI progressText;        // Опционально: текст прогресса
    public string sceneName = "MainLevel";
    public float minLoadingTime = 3f;           // Минимальное время загрузки
    public Image fadeScreen;                    // ← НОВЫЙ: UI Image для затемнения (черный, прозрачный)

    private float loadingTimer = 0f;

    void Start()
    {
        if (fadeScreen == null)
        {
            Debug.LogError("Fade screen image not assigned!");
            return;
        }

        // Изначально делаем затемнение прозрачным
        fadeScreen.color = new Color(0, 0, 0, 0);
        
        StartCoroutine(LoadSceneAsync());
        
        YG2.StickyAdActivity(true);
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Запрещаем мгновенный переход

        // Ждём, пока загрузка не достигнет 90% и не пройдёт minLoadingTime
        while (!asyncLoad.isDone || loadingTimer < minLoadingTime)
        {
            loadingTimer += Time.deltaTime;

            // Прогресс загрузки (0-90% → 0-100%)
            float loadProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Искусственное замедление, чтобы загрузка не завершалась слишком быстро
            float displayedProgress = Mathf.Min(loadProgress, loadingTimer / minLoadingTime);

            // Обновляем UI
            progressBar.value = displayedProgress;

            // (Опционально) Выводим текст в процентах
            if (progressText != null)
                progressText.text = $"{(int)(displayedProgress * 100)}%";

            // Если загрузка реально завершена и прошло minLoadingTime — готовы к затемнению
            if (loadProgress >= 1f && loadingTimer >= minLoadingTime)
            {
                // Теперь останавливаемся и начинаем эффект затемнения
                yield return StartCoroutine(FadeToBlackAndBack());
                
                // После затемнения — разрешаем активацию сцены
                asyncLoad.allowSceneActivation = true;
                break;
            }

            yield return null;
        }
    }

    // Анимация затемнения: плавное затемнение → удержание → плавное исчезновение
    IEnumerator FadeToBlackAndBack()
    {
        const float fadeInDuration = 0.3f;
        const float holdDuration = 2f;
        const float fadeOutDuration = 0.3f;

        // Шаг 1: Плавное затемнение (от прозрачного к черному)
        float timeElapsed = 0f;
        while (timeElapsed < fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeInDuration);
            fadeScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeScreen.color = Color.black; // Убедимся, что полностью чёрный

        // Шаг 2: Удерживаем чёрный экран 2 секунды
        yield return new WaitForSeconds(holdDuration);

        // Шаг 3: Плавное исчезновение затемнения (от черного к прозрачному)
        timeElapsed = 0f;
        while (timeElapsed < fadeOutDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeOutDuration);
            fadeScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeScreen.color = new Color(0, 0, 0, 0); // Полностью прозрачный
    }
}