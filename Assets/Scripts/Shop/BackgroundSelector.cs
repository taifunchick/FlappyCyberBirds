using UnityEngine;
using UnityEngine.UI;

public class BackgroundSelector : MonoBehaviour
{
    [SerializeField] private Button[] backgroundButtons;
    [SerializeField] private Image[] previewImages;
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private string playerPrefsKey = "SelectedBackground";

    private void Start()
    {
        ApplyPreviewSprites();
        UpdateButtonStates(GetSelectedIndex());
    }

    public void SelectBackground(int index)
    {
        if (backgroundSprites == null || backgroundSprites.Length == 0)
        {
            return;
        }

        int clampedIndex = Mathf.Clamp(index, 0, backgroundSprites.Length - 1);
        PlayerPrefs.SetInt(playerPrefsKey, clampedIndex);
        PlayerPrefs.Save();
        UpdateButtonStates(clampedIndex);
    }

    private void ApplyPreviewSprites()
    {
        if (previewImages == null || backgroundSprites == null)
        {
            return;
        }

        for (int i = 0; i < previewImages.Length && i < backgroundSprites.Length; i++)
        {
            if (previewImages[i] != null)
            {
                previewImages[i].sprite = backgroundSprites[i];
            }
        }
    }

    private int GetSelectedIndex()
    {
        int index = PlayerPrefs.GetInt(playerPrefsKey, 0);
        if (backgroundSprites == null || backgroundSprites.Length == 0)
        {
            return 0;
        }

        return Mathf.Clamp(index, 0, backgroundSprites.Length - 1);
    }

    private void UpdateButtonStates(int selectedIndex)
    {
        if (backgroundButtons == null)
        {
            return;
        }

        for (int i = 0; i < backgroundButtons.Length; i++)
        {
            Button button = backgroundButtons[i];
            if (button == null)
            {
                continue;
            }

            ColorBlock colors = button.colors;
            colors.normalColor = (i == selectedIndex) ? Color.green : Color.white;
            button.colors = colors;
        }
    }
}
