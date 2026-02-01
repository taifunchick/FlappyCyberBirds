using UnityEngine;
using UnityEngine.UI;

public class BackgroundApplier : MonoBehaviour
{
    [SerializeField] private Image[] targetImages;
    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private string playerPrefsKey = "SelectedBackground";

    private void Start()
    {
        ApplyBackground();
    }

    public void ApplyBackground()
    {
        if (backgroundSprites == null || backgroundSprites.Length == 0)
        {
            return;
        }

        int index = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey, 0), 0, backgroundSprites.Length - 1);
        Sprite selectedSprite = backgroundSprites[index];

        Image[] images = targetImages;
        if (images == null || images.Length == 0)
        {
            images = FindBackgroundImages();
        }

        foreach (Image image in images)
        {
            if (image != null)
            {
                image.sprite = selectedSprite;
            }
        }
    }

    private Image[] FindBackgroundImages()
    {
        Image[] allImages = FindObjectsOfType<Image>();
        return System.Array.FindAll(allImages, image => image != null && image.name == "Bg");
    }
}
