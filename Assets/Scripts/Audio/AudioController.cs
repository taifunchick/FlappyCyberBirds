using UnityEngine;
using UnityEngine.UI;
using YG;

public class AudioController : MonoBehaviour
{
    public Slider musicSlider;      // Ссылка на слайдер музыки
    public Slider sfxSlider;        // Ссылка на слайдер звуков

    public AudioSource musicSource; // Источник музыки
    public AudioSource sfxSource;   // Источник звуков

    // 👇 НОВОЕ: Общий звук нажатия кнопки (один на всю игру)
    public AudioClip buttonClickSound; // Перетаскиваем сюда звук в инспекторе

    public static AudioController _instance;

    private void Awake()
    {
        // Singleton: только один экземпляр на весь проект
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Проверяем обязательные ссылки
        ValidateReferences();

        // Подписываемся на изменения слайдеров
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSfxVolume);

        // Загружаем сохранённые значения
        LoadVolumes();

        YG2.StickyAdActivity(true);
    }

    private void ValidateReferences()
    {
        if (musicSlider == null) Debug.LogError("❌ MusicSlider не назначен в инспекторе!");
        if (sfxSlider == null) Debug.LogError("❌ SfxSlider не назначен в инспекторе!");
        if (musicSource == null) Debug.LogError("❌ MusicSource не назначен в инспекторе!");
        if (sfxSource == null) Debug.LogError("❌ SfxSource не назначен в инспекторе!");
        if (buttonClickSound == null) Debug.LogWarning("⚠️ ButtonClickSound не назначен — звук кнопок не будет играть!");
    }

    // ✅ Безопасная установка громкости музыки
    public void UpdateMusicVolume(float value)
    {
        float safeValue = SafeVolume(value);
        musicSource.volume = safeValue;
        SaveVolume("Music", safeValue);
    }

    // ✅ Безопасная установка громкости звуков
    public void UpdateSfxVolume(float value)
    {
        float safeValue = SafeVolume(value);
        sfxSource.volume = safeValue;
        SaveVolume("Sfx", safeValue);
    }

    // ✅ Защита от NaN и Infinity
    private float SafeVolume(float value)
    {
        // Проверяем на некорректные значения
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            Debug.LogWarning($"⚠️ Обнаружен недопустимый объём звука: {value}. Установлено значение по умолчанию 0.5f.");
            return 0.5f;
        }

        // Ограничиваем диапазон [0, 1]
        return Mathf.Clamp(value, 0f, 1f);
    }

    // Сохраняем громкость в PlayerPrefs
    private void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    // Загружаем громкость из PlayerPrefs с безопасной обработкой
    private void LoadVolumes()
    {
        float musicVol = PlayerPrefs.GetFloat("Music", 0.5f);
        float sfxVol = PlayerPrefs.GetFloat("Sfx", 0.5f);

        // ✅ Защищаем значения при загрузке
        musicSlider.value = SafeVolume(musicVol);
        sfxSlider.value = SafeVolume(sfxVol);

        // Принудительно применяем громкость (на случай, если слайдер не вызвал метод)
        UpdateMusicVolume(musicSlider.value);
        UpdateSfxVolume(sfxSlider.value);

        Debug.Log($"✅ Громкость загружена: Музыка={musicVol}, Звуки={sfxVol}");
    }

    // --- Дополнительно ---

    public void ToggleMute(bool mute)
    {
        musicSource.mute = mute;
        sfxSource.mute = mute;
    }

    public void ResetVolumes()
    {
        UpdateMusicVolume(0.5f);
        UpdateSfxVolume(0.5f);
    }

    // 👇 Публичный доступ к звуку кнопки — может пригодиться для других систем
    public AudioClip GetButtonClickSound() => buttonClickSound;
}