using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characterButtons;
    public string[] characterNames;

    private void Start()
    {
        string selectedChar = PlayerPrefs.GetString("SelectedCharacter", "");
        UpdateButtonStates(selectedChar);
    }

    public void OnCharacterSelected(string characterName)
    {
        PlayerPrefs.SetString("SelectedCharacter", characterName);
        PlayerPrefs.Save();
        UpdateButtonStates(characterName);
    }

    private void UpdateButtonStates(string selectedCharacter)
    {
        if (characterButtons == null || characterNames == null)
        {
            return;
        }

        for (int i = 0; i < characterButtons.Length && i < characterNames.Length; i++)
        {
            Button button = characterButtons[i].GetComponent<Button>();
            string charName = characterNames[i];

            if (button != null)
            {
                TMP_Text label = button.GetComponentInChildren<TMP_Text>();
                if (label != null)
                {
                    string key = charName == selectedCharacter ? "selected" : "select";
                    LocalizedText localized = label.GetComponent<LocalizedText>();
                    if (localized == null)
                    {
                        localized = label.gameObject.AddComponent<LocalizedText>();
                    }

                    localized.SetKey(key);
                    label.text = GetLocalized(key, key);
                }

                ColorBlock colors = button.colors;
                colors.normalColor = (charName == selectedCharacter) ? Color.green : Color.white;
                button.colors = colors;
            }
        }
    }

    private static string GetLocalized(string key, string fallback)
    {
        var manager = CSVLocalizationManager._instance;
        return manager != null ? manager.GetText(key) : fallback;
    }
}
