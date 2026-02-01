using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;
    private TMP_Text textComponent;

    public string Key => key;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        UpdateText();
    }

    public void SetKey(string newKey)
    {
        key = newKey;
    }

    public void UpdateText()
    {
        if (string.IsNullOrWhiteSpace(key) || textComponent == null)
        {
            return;
        }

        var manager = CSVLocalizationManager._instance;
        if (manager == null)
        {
            return;
        }

        textComponent.text = manager.GetText(key);
    }
}
