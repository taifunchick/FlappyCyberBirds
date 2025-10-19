using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnButtonClick : MonoBehaviour
{
    // Убираем локальные AudioSource и AudioClip
    // Вместо этого используем глобальный AudioController

    [SerializeField] private Button button; // Опционально: можно назначить в инспекторе

    private void Awake()
    {
        // Если кнопка не назначена — ищем её как дочерний элемент
        if (button == null)
        {
            button = GetComponent<Button>();
            if (button == null)
            {
                button = gameObject.GetComponentInChildren<Button>();
            }
        }

        // Подписываемся на событие нажатия кнопки
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
        else
        {
            Debug.LogWarning("Button not assigned and not found in children!");
        }
    }

    public void PlaySound()
    {
        // Проверяем, существует ли экземпляр AudioController
        if (AudioController._instance == null)
        {
            Debug.LogError("❌ AudioController not found in scene! Sound won't play.");
            return;
        }

        // Проверяем, назначен ли звук нажатия в AudioController
        if (AudioController._instance.buttonClickSound == null)
        {
            Debug.LogWarning("❌ No button click sound assigned in AudioController!");
            return;
        }

        // Воспроизводим звук через глобальный sfxSource (который уже управляем через слайдер!)
        AudioController._instance.sfxSource.PlayOneShot(AudioController._instance.buttonClickSound);
    }
}