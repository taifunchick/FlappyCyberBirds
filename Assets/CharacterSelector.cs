using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characterButtons; // Массив кнопок (каждая — Button)
    public string[] characterNames;       // Имена персонажей (должны совпадать с именами префабов)
    
    private void Start()
    {
        // Загружаем последний выбранный персонаж из PlayerPrefs
        string selectedChar = PlayerPrefs.GetString("SelectedCharacter", "");
        
        // Обновляем все кнопки
        UpdateButtonStates(selectedChar);
    }

    public void OnCharacterSelected(string characterName)
    {
        // Сохраняем выбор
        PlayerPrefs.SetString("SelectedCharacter", characterName);
        PlayerPrefs.Save();
        
        // Обновляем состояние всех кнопок
        UpdateButtonStates(characterName);
    }

    private void UpdateButtonStates(string selectedCharacter)
    {
        if (characterButtons == null || characterNames == null) return;

        for (int i = 0; i < characterButtons.Length && i < characterNames.Length; i++)
        {
            Button button = characterButtons[i].GetComponent<Button>();
            string charName = characterNames[i];
            
            if (button != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = 
                    (charName == selectedCharacter) ? "Выбран" : "Выбрать";
                
                // Можно добавить визуальное выделение (например, цвет фона)
                ColorBlock colors = button.colors;
                colors.normalColor = (charName == selectedCharacter) ? Color.green : Color.white;
                button.colors = colors;
            }
        }
    }
}