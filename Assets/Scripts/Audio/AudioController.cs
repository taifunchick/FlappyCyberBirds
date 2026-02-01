using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class AudioController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip buttonClickSound;

    public static AudioController _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += HandleSceneLoaded;

        ValidateReferences();

        BindSliderListeners();

        LoadVolumes();

        YG2.StickyAdActivity(true);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindSliders();
        BindSliderListeners();
        LoadVolumes();
    }

    private void ValidateReferences()
    {
        if (musicSlider == null) Debug.LogError("MusicSlider not assigned.");
        if (sfxSlider == null) Debug.LogError("SfxSlider not assigned.");
        if (musicSource == null) Debug.LogError("MusicSource not assigned.");
        if (sfxSource == null) Debug.LogError("SfxSource not assigned.");
    }

    private void RebindSliders()
    {
        if (musicSlider == null)
        {
            GameObject musicObject = GameObject.Find("MusicSlider");
            if (musicObject != null)
            {
                musicSlider = musicObject.GetComponent<Slider>();
            }
        }

        if (sfxSlider == null)
        {
            GameObject sfxObject = GameObject.Find("SfxSlider");
            if (sfxObject != null)
            {
                sfxSlider = sfxObject.GetComponent<Slider>();
            }
        }
    }

    private void BindSliderListeners()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(UpdateSfxVolume);
            sfxSlider.onValueChanged.AddListener(UpdateSfxVolume);
        }
    }

    public void UpdateMusicVolume(float value)
    {
        float safeValue = SafeVolume(value);
        if (musicSource != null)
        {
            musicSource.volume = safeValue;
        }
        SaveVolume("Music", safeValue);
    }

    public void UpdateSfxVolume(float value)
    {
        float safeValue = SafeVolume(value);
        if (sfxSource != null)
        {
            sfxSource.volume = safeValue;
        }
        SaveVolume("Sfx", safeValue);
    }

    private float SafeVolume(float value)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            return 0.5f;
        }

        return Mathf.Clamp(value, 0f, 1f);
    }

    private void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    private void LoadVolumes()
    {
        float musicVol = PlayerPrefs.GetFloat("Music", 0.5f);
        float sfxVol = PlayerPrefs.GetFloat("Sfx", 0.5f);

        float safeMusic = SafeVolume(musicVol);
        float safeSfx = SafeVolume(sfxVol);

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(safeMusic);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(safeSfx);
        }

        UpdateMusicVolume(safeMusic);
        UpdateSfxVolume(safeSfx);
    }

    public void ToggleMute(bool mute)
    {
        if (musicSource != null)
        {
            musicSource.mute = mute;
        }

        if (sfxSource != null)
        {
            sfxSource.mute = mute;
        }
    }

    public void ResetVolumes()
    {
        UpdateMusicVolume(0.5f);
        UpdateSfxVolume(0.5f);
    }

    public AudioClip GetButtonClickSound() => buttonClickSound;
}
