using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string key; 
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    public void UpdateText()
    {
        if (CSVLocalizationManager._instance != null)
        {
            textComponent.text = CSVLocalizationManager._instance.GetText(key);
        }
        else
        {
            Debug.LogError("CSVLocalizationManager instance not found in the scene!");
        }
    }
}