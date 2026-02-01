using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CSVLocalizationManager : MonoBehaviour
{
    public static CSVLocalizationManager _instance;
    public TextAsset csvFile;
    public string[] availableLanguages = { "Русский", "English" };
    public int currentLanguageIndex;
    public TextMeshProUGUI languageText;

    private readonly Dictionary<string, Dictionary<string, string>> localizationData = new Dictionary<string, Dictionary<string, string>>();
    private string currentLanguage;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCSV();
        LoadLanguage();
        UpdateLanguageText();
    }

    private void Start()
    {
        RebindLanguageText();
        UpdateAllText();
        UpdateLanguageText();
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        RebindLanguageText();
        UpdateAllText();
        UpdateLanguageText();
    }

    void LoadCSV()
    {
        localizationData.Clear();

        string[] lines = csvFile.text.Split('\n');
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file is empty or invalid.");
            return;
        }

        string[] languages = lines[0].Split(',');
        if (languages.Length - 1 != availableLanguages.Length)
        {
            Debug.LogWarning("Number of languages in CSV (" + (languages.Length - 1) + ") does not match availableLanguages array (" + availableLanguages.Length + ").");
        }

        for (int i = 1; i < languages.Length; i++)
        {
            string language = languages[i].Trim();
            if (!availableLanguages.Contains(language))
            {
                Debug.LogWarning("Language '" + language + "' in CSV is not in availableLanguages array.");
            }
            localizationData[language] = new Dictionary<string, string>();
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length != languages.Length)
            {
                Debug.LogWarning("Invalid line in CSV file: " + lines[i]);
                continue;
            }

            string key = values[0].Trim();
            for (int j = 1; j < values.Length; j++)
            {
                string language = languages[j].Trim();
                if (localizationData.ContainsKey(language))
                {
                    localizationData[language][key] = values[j].Trim();
                }
            }
        }
    }


    public void SetLanguage(int index)
    {
        if (index >= 0 && index < availableLanguages.Length)
        {
            currentLanguageIndex = index;
            currentLanguage = availableLanguages[currentLanguageIndex];
            SaveLanguage();
            UpdateAllText();
            UpdateLanguageText();
        }
        else
        {
            Debug.LogError("Invalid language index: " + index);
        }
    }

    public void NextLanguage()
    {
        currentLanguageIndex = (currentLanguageIndex + 1) % availableLanguages.Length;
        SetLanguage(currentLanguageIndex);
    }

    public void PreviousLanguage()
    {
        currentLanguageIndex = (currentLanguageIndex - 1 + availableLanguages.Length) % availableLanguages.Length;
        SetLanguage(currentLanguageIndex);
    }

    public string GetText(string key)
    {
        currentLanguage = availableLanguages[currentLanguageIndex];
        if (localizationData.ContainsKey(currentLanguage) && localizationData[currentLanguage].ContainsKey(key))
        {
            return localizationData[currentLanguage][key];
        }
        else
        {
            Debug.LogWarning("Key not found: " + key + " in language: " + currentLanguage);
            return key; 
        }
    }

    void SaveLanguage()
    {
        PlayerPrefs.SetInt("LanguageIndex", currentLanguageIndex);
        PlayerPrefs.Save();
    }

    void LoadLanguage()
    {
        if (PlayerPrefs.HasKey("LanguageIndex"))
        {
            currentLanguageIndex = PlayerPrefs.GetInt("LanguageIndex");
            if (currentLanguageIndex < 0 || currentLanguageIndex >= availableLanguages.Length)
            {
                Debug.LogWarning("Invalid language index in PlayerPrefs. Resetting to 0.");
                currentLanguageIndex = 0;
                SaveLanguage();
            }
        }
        else
        {
            SaveLanguage();
        }

        currentLanguage = availableLanguages[currentLanguageIndex];
    }

    public void UpdateAllText()
    {
        TMP_Text[] tmpTexts = FindObjectsOfType<TMP_Text>(true);
        foreach (TMP_Text tmpText in tmpTexts)
        {
            if (tmpText == null)
            {
                continue;
            }

            LocalizedText localizedText = tmpText.GetComponent<LocalizedText>();
            if (localizedText == null)
            {
                localizedText = tmpText.gameObject.AddComponent<LocalizedText>();
            }

            if (string.IsNullOrWhiteSpace(localizedText.Key))
            {
                localizedText.SetKey(tmpText.text.Trim());
            }

            localizedText.UpdateText();
        }
    }

    private void UpdateLanguageText()
    {
        if (languageText != null)
        {
            languageText.text = availableLanguages[currentLanguageIndex];
        }
    }

    private void RebindLanguageText()
    {
        if (languageText != null)
        {
            return;
        }

        GameObject languageObject = GameObject.FindWithTag("LanguageText");
        if (languageObject != null)
        {
            languageText = languageObject.GetComponent<TextMeshProUGUI>();
        }
    }
}
