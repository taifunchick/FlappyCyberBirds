using UnityEngine;
using UnityEngine.UI;

public class PipeSkinSelector : MonoBehaviour
{
    [SerializeField] private Button[] pipeButtons;
    [SerializeField] private Image[] previewImages;
    [SerializeField] private Sprite[] pipeSprites;
    [SerializeField] private string playerPrefsKey = "SelectedPipeSkin";

    private void Start()
    {
        ApplyPreviewSprites();
        UpdateButtonStates(GetSelectedIndex());
    }

    public void SelectPipeSkin(int index)
    {
        if (pipeSprites == null || pipeSprites.Length == 0)
        {
            return;
        }

        int clampedIndex = Mathf.Clamp(index, 0, pipeSprites.Length - 1);
        PlayerPrefs.SetInt(playerPrefsKey, clampedIndex);
        PlayerPrefs.Save();
        UpdateButtonStates(clampedIndex);
    }

    private void ApplyPreviewSprites()
    {
        if (previewImages == null || pipeSprites == null)
        {
            return;
        }

        for (int i = 0; i < previewImages.Length && i < pipeSprites.Length; i++)
        {
            if (previewImages[i] != null)
            {
                previewImages[i].sprite = pipeSprites[i];
            }
        }
    }

    private int GetSelectedIndex()
    {
        int index = PlayerPrefs.GetInt(playerPrefsKey, 0);
        if (pipeSprites == null || pipeSprites.Length == 0)
        {
            return 0;
        }

        return Mathf.Clamp(index, 0, pipeSprites.Length - 1);
    }

    private void UpdateButtonStates(int selectedIndex)
    {
        if (pipeButtons == null)
        {
            return;
        }

        for (int i = 0; i < pipeButtons.Length; i++)
        {
            Button button = pipeButtons[i];
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
